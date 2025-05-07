import React, { useEffect, useState, type JSX } from 'react';
import { Navigate } from 'react-router-dom';
import userManager from './authConfig'; // adjust path if needed

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

  useEffect(() => {
    userManager.getUser().then(user => {
      setIsAuthenticated(!!user && !user.expired);
    });
  }, []);

  if (isAuthenticated === null) {
    return <div>Loading...</div>; // or spinner
  }

  return isAuthenticated ? children : <Navigate to="/admin/dashboard" replace />;
};

export default PrivateRoute;
