import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import userManager from '../../auth/authConfig';

const Callback = () => {
  const navigate = useNavigate();

  useEffect(() => {
    userManager
      .signinRedirectCallback()
      .then(user => {
        const roles = user.profile?.role;

        const isAdmin =
          Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';

        if (!isAdmin) {
          // ❌ Not an admin: logout and block login
          alert('Access denied: Only Admins are allowed');
          userManager.removeUser(); // Clear localStorage
          userManager.signoutRedirect(); // Optional: force sign out of AuthServer
          return;
        }

        // ✅ Admin: continue
        navigate('/admin/dashboard');
      })
      .catch(err => {
        console.error('Login error:', err);
      });
  }, []);

  return <p>Processing login...</p>;
};

export default Callback;
