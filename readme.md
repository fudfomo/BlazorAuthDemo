### Register the API (BlazorAuthDemo.Api)

1.  Navigate to the [Azure portal](https://portal.azure.com).

2.  Select the **App Registrations** blade on the left, then select **New registration**.

3.  In the **Register an application page** that appears, enter your application's registration information:
    1.  In the **Name** section, enter a meaningful application name that will be displayed to users of the app: `BlazorAuthDemo.Api`
    2.  Under **Supported account types**, select **Accounts in this organizational directory only**
    3.  Select **Register** to create the application.

4.  In the **Overview** blade, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.

5.  In the app's registration screen, select the **Expose an API** blade to the left to open the page where you can publish the permission as an API for which client applications can obtain [access tokens](https://aka.ms/access-tokens) for. The first thing that we need to do is to declare the unique [resource](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-auth-code-flow) URI that the clients will be using to obtain access tokens for this API. To declare an resource URI(Application ID URI), follow the following steps:
    1.  Select **Add** next to the **Application ID URI** to generate a URI that is unique for this app.
    2.  For this sample, accept the proposed Application ID URI (`api://{Id}`) by selecting **Save**. Read more about Application ID URI at [Validation differences by supported account types (signInAudience)](https://docs.microsoft.com/azure/active-directory/develop/supported-accounts-validation).

### Publish Delegated Permissions

1. All APIs must publish a minimum of one scope, also called Delegated Permission, for the client apps to obtain an access token for a user successfully. To publish a scope, follow these steps:

2. Select Add a scope button open the Add a scope screen and Enter the values as indicated below:
    1. For Scope name, use **BlazorAuthDemo.Read**.
    2. Select Admins and users options for Who can consent?.
    3. For Admin consent display name type in `Read BlazorAuthDemo.Api`.
    4. For Admin consent description type in `Allow the app to read BlazorAuthDemo.Api`.
    5. For User consent display name type in `Read via the BlazorAuthDemo.Api`.
    6. For User consent description type in `Allow the app to read via the BlazorAuthDemo.Api`.
    7. Keep State as Enabled.
    8. Select the Add scope button on the bottom to save this scope.
    9. Repeat the steps above for another scope named **BlazorAuthDemo.ReadWrite**

1. Select the Manifest blade on the left.
    1. Set accessTokenAcceptedVersion property to 2.
    2. Select on Save.

### Publish Application Permissions

1. All APIs should publish a minimum of one App role for applications, also called Application Permission, for the client apps to obtain an access token as themselves, i.e. when they are not signing-in a user. Application permissions are the type of permissions that APIs should publish when they want to enable client applications to successfully authenticate as themselves and not need to sign-in users. To publish an application permission, follow these steps:

2. Still on the same app registration, select the App roles blade to the left.

3. Select Create app role:

    1. For Display name, enter `BlazorAuthDemo.Read.All`.
    2. For Allowed member types, choose **Application** to ensure other applications can be granted this permission.
    3. For Value, enter `BlazorAuthDemo.Read.All`.
    4. For Description, enter `Allow the app to read using the BlazorAuthDemo.Api`.
    5. Select Apply to save your changes.
    
Repeat the steps above for another app permission named **BlazorAuthDemo.ReadWrite.All**

### Configure Optional Claims
1. Still on the same app registration, select the Token configuration blade to the left.
2. Select Add optional claim:
    1. Select optional claim type, then choose Access.
    2. Select the optional claim idtyp.
Indicates token type. This claim is the most accurate way for an API to determine if a token is an app token or an app+user token. This is not issued in tokens issued to users.

    3. Select Add to save your changes.

### Add Owners to Api
1. Select **Owners**
2. Click **Add Owners**
3. Select your account
*This step is required for the Api to show up in the API Permissions step of client.*    

### appsettings.development.json

1.  Store the App Registration values in the appsettings.development.json and for security reasons make sure to not check in this file:

  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "ClientId": "Client ID from API App Registration",
    "TenantId": "Tenant ID from APO App Registration"
  },

2.  When deployed to Azure, the API should read these settings from Environment Variables or ideally, from KeyVault

***

### Register the client app (BlazorAuthDemo.Client) 

1.  Navigate to the [Azure portal](https://portal.azure.com)

2. Select the App Registrations, then select New registration.

3. In the Register an application page that appears, enter your application's registration information:
    1. In the Name section, enter a meaningful application name that will be displayed to users of the app: `BlazorAuthDemo.Client`
    2. Under Supported account types, select Accounts in this organizational directory only
    3. Select **Register** to create the application.

4. In the Overview blade, find and note the Application (client) ID. You use this value in your app's configuration file(s) later in your code.
5. In the app's registration screen, select the Authentication blade to the left.
6. If you don't have a platform added, select Add a platform and select the Web option.
    1. In the Redirect URI section enter the following redirect URI: `https://localhost:44321/signin-oidc`
    2. In the Front-channel logout URL section, set it to `https://localhost:44321/signout-oidc.`
    3. Check **ID tokens (used for implicit and hybrid flows)**
    4. Click **Configure** to save your changes.

7. In the app's registration screen, select the Certificates & secrets blade in the left to open the page where you can generate secrets and upload certificates.
8. In the Client secrets section, select New client secret:
    1. Type a key description (for instance `BlazorClientDemoSecret`).
    2. Select one of the available key durations (6 months, 12 months or Custom) as per your security posture.
    3. The generated key value will be displayed when you select the Add button. Copy and save the generated value for use in later steps.
    4. You'll need this key later in your code's configuration files. This key value will not be displayed again, and is not retrievable by any other means, so make sure to note it from the Azure portal before navigating to any other screen or blade.

### Set API Permissions
1. Since this app signs-in users, we will now proceed to select **delegated permissions**, which is is required by apps signing-in users.
   1. In the app's registration screen, select the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs:
   1. Select the **Add a permission** button and then:
   1. Ensure that the **My APIs** tab is selected.
   1. In the list of APIs, select the API `BlazorAuthDemo.Api`.
      * Since this app signs-in users, we will now proceed to select **delegated permissions**, which is requested by apps that signs-in users.
      * In the **Delegated permissions** section, select **BlazorAuthDemo.Read**, **BlazorAuthDemo.ReadWrite** in the list. Use the search box if necessary.
   1. Select the **Add permissions** button at the bottom.

#### Configure Optional Claims

1. Still on the same app registration, select the **Token configuration** blade to the left.
1. Select **Add optional claim**:
    1. Select **optional claim type**, then choose **ID**.
    1. Select the optional claim **acct**.
    > Provides user's account status in tenant. If the user is a **member** of the tenant, the value is *0*. If they're a **guest**, the value is *1*.
    1. Select **Add** to save your changes.

#### appsettings.development.json

1.  Store the App Registration values in the appsettings.development.json and for security reasons make sure to not check in this file:

  ~~~
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "https://portal.azure.com/",
    "ClientId": "Client ID from API App Registration",
    "TenantId": "Tenant ID from APO App Registration"
    "ClientSecret": "Client Secret from App Registrations",
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-oidc"
  },
  "BlazorAuthDemo.Api": {
    "Scopes": [ "api://{id}/BlazorAuthDemo.ReadWrite", "api://{id}/BlazorAuthDemo.Read" ],
    "BaseUrl": "https://localhost:7251/",
    "RelativePath": "api"
  },
~~~

2.  When deployed to Azure, the API should read these settings from Environment Variables or ideally, from KeyVault

NOTE: the BaseUrl should be the same as the url your api is running on.

***
