# Azure Entra ID Authentication Guide

## Overview

The ExBot API is secured with Azure Entra ID (formerly Azure Active Directory) for enterprise-grade authentication and authorization.

## Prerequisites

1. Azure subscription
2. Azure Entra ID tenant
3. Permissions to create app registrations

## Setup Steps

### 1. Register Application in Azure Entra ID

1. Navigate to [Azure Portal](https://portal.azure.com)
2. Go to **Azure Entra ID** → **App registrations** → **New registration**
3. Configure:
   - **Name**: `ExBot API`
   - **Supported account types**: Choose based on your needs
   - **Redirect URI**: Leave empty for now
4. Click **Register**

### 2. Configure API Permissions

1. In your app registration, go to **Expose an API**
2. Click **Add a scope**
3. Set **Application ID URI**: `api://YOUR_CLIENT_ID`
4. Add scope:
   - **Scope name**: `access_as_user`
   - **Who can consent**: Admins and users
   - **Admin consent display name**: Access ExBot API
   - **Admin consent description**: Allows the app to access ExBot API as the user
   - **User consent display name**: Access ExBot API
   - **User consent description**: Allows the app to access ExBot API on your behalf
5. Click **Add scope**

### 3. Create Client Secret (Optional - for service-to-service)

1. Go to **Certificates & secrets**
2. Click **New client secret**
3. Add description and set expiration
4. **Copy the secret value** (you won't see it again!)

### 4. Configure API Application

Update `appsettings.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "YOUR_TENANT_ID",
    "ClientId": "YOUR_CLIENT_ID",
    "Audience": "api://YOUR_CLIENT_ID"
  }
}
```

**Finding your values:**
- **TenantId**: Azure Entra ID → Overview → Tenant ID
- **ClientId**: App registration → Overview → Application (client) ID

### 5. Environment Variables (Production)

For production, use environment variables or Azure Key Vault:

```bash
export AzureAd__TenantId="your-tenant-id"
export AzureAd__ClientId="your-client-id"
```

Or in `appsettings.Production.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "#{AZURE_TENANT_ID}#",
    "ClientId": "#{AZURE_CLIENT_ID}#"
  }
}
```

## Client Application Setup

### Web Application

Register a separate app for your web client:

1. Create new app registration for web app
2. Add redirect URI: `https://your-web-app.com/signin-oidc`
3. Add API permissions:
   - **My APIs** → **ExBot API** → **access_as_user**
4. Grant admin consent

### Mobile Application

For React Native/MAUI mobile apps:

1. Create new app registration for mobile
2. Set **Platform**: Mobile and desktop applications
3. Add redirect URI: `msauth://com.yourcompany.exbot/callback`
4. Add API permissions:
   - **My APIs** → **ExBot API** → **access_as_user**

### Postman Testing

1. Authorization Type: **OAuth 2.0**
2. Configure:
   - **Auth URL**: `https://login.microsoftonline.com/YOUR_TENANT_ID/oauth2/v2.0/authorize`
   - **Access Token URL**: `https://login.microsoftonline.com/YOUR_TENANT_ID/oauth2/v2.0/token`
   - **Client ID**: Your app's client ID
   - **Scope**: `api://YOUR_API_CLIENT_ID/access_as_user`
   - **Client Authentication**: Send as Basic Auth header

## API Protection

### Controller-Level Authorization

All controllers are protected by default:

```csharp
[Authorize]
[RequiredScope("access_as_user")]
public class UsersController : ControllerBase
{
    // All endpoints require authentication
}
```

### Allow Anonymous Access

For specific endpoints that don't require auth:

```csharp
[AllowAnonymous]
[HttpGet("public/health")]
public IActionResult Health()
{
    return Ok("Healthy");
}
```

### Role-Based Authorization

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(Guid id)
{
    // Only users with Admin role can access
}
```

### Scope-Based Authorization

```csharp
[RequiredScope("User.Read", "User.Write")]
[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(Guid id)
{
    // Requires both scopes
}
```

## Testing Authentication

### Swagger UI

1. Start the API
2. Navigate to `http://localhost:5050`
3. Click **Authorize**
4. Click **Authorize** on the OAuth2 dialog
5. Sign in with Azure Entra ID credentials
6. Test protected endpoints

### Get Access Token via cURL

```bash
# Get token
curl -X POST https://login.microsoftonline.com/YOUR_TENANT_ID/oauth2/v2.0/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=YOUR_CLIENT_ID" \
  -d "scope=api://YOUR_API_CLIENT_ID/access_as_user" \
  -d "grant_type=client_credentials" \
  -d "client_secret=YOUR_CLIENT_SECRET"

# Use token
curl -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  http://localhost:5050/api/Users
```

## Claims and User Information

Access user information from the token:

```csharp
[HttpGet("me")]
public IActionResult GetCurrentUser()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var name = User.FindFirst(ClaimTypes.Name)?.Value;
    
    return Ok(new { userId, email, name });
}
```

## Security Best Practices

1. **Always use HTTPS** in production
2. **Validate token issuer** - Already configured
3. **Check token expiration** - Automatic with Microsoft.Identity.Web
4. **Use minimal scopes** - Request only what's needed
5. **Rotate secrets regularly** - Set expiration on client secrets
6. **Monitor sign-ins** - Use Azure AD audit logs
7. **Enable MFA** - For production tenant users
8. **Use Managed Identity** - For Azure-hosted services

## Troubleshooting

### Common Issues

**401 Unauthorized:**
- Check token is included in Authorization header
- Verify token hasn't expired
- Confirm app registration is correct

**403 Forbidden:**
- Check user has required scopes/roles
- Verify scope name matches configuration

**Token Validation Failed:**
- Verify TenantId and ClientId in appsettings.json
- Check token audience matches API ClientId
- Ensure clock sync between client and server

### Debug Authentication

Enable detailed auth logging:

```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.Authentication": "Debug",
      "Microsoft.Identity": "Debug"
    }
  }
}
```

## Resources

- [Microsoft Identity Platform Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)
- [Azure Entra ID Overview](https://docs.microsoft.com/en-us/azure/active-directory/)
- [Microsoft.Identity.Web Documentation](https://github.com/AzureAD/microsoft-identity-web)
