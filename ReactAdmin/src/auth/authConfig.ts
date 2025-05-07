import type { UserManagerSettings } from 'oidc-client-ts';
import { UserManager, WebStorageStateStore } from 'oidc-client-ts';

const authConfig: UserManagerSettings = {
  authority: 'https://localhost:7000', // URL of your OpenIddict AuthServer
  client_id: 'react-client',     // This must match the client registered on your AuthServer
  redirect_uri: 'https://localhost:7054/signin-oidc', // Must be listed in AuthServer's allowed redirect URIs
  post_logout_redirect_uri: 'https://localhost:7054/', // Must be listed in AuthServer's allowed post logout redirect URIs
  response_type: 'code', // Authorization Code Flow
  scope: 'openid profile email roles api resource_server_1', // Scopes your AuthServer supports
  userStore: new WebStorageStateStore({ store: window.localStorage }),
};

const userManager = new UserManager(authConfig);
export default userManager;