import React from 'react';
import { useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Layout, Menu } from 'antd';
import {
  AppstoreOutlined,
  PlusOutlined,
  UnorderedListOutlined,
  LoginOutlined,
  LogoutOutlined,
} from '@ant-design/icons';

import userManager from '../../auth/authConfig'; // path may vary

const { Sider } = Layout;

const AdminSidebar: React.FC = () => {
  const location = useLocation();
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    userManager.getUser().then(user => {
      setIsLoggedIn(!!user && !user.expired);
    });
  }, []);

  const handleLogin = () => {
    userManager.signinRedirect();
  };

  const handleLogout = () => {
    userManager.signoutRedirect();
  };

  return (
    <Sider collapsible>
      <div className="demo-logo" style={{ height: 32, margin: 16, color: 'white', fontWeight: 'bold' }}>
        Admin Panel
      </div>
      <Menu
        theme="dark"
        mode="inline"
        selectedKeys={[location.pathname]} // highlight current menu item
      >
        <Menu.Item key="/" icon={<AppstoreOutlined />}>
          <Link to="/">Dashboard</Link>
        </Menu.Item>

        <Menu.Item key="/admin/phoneadd" icon={<PlusOutlined />}>
          <Link to="/admin/phoneadd">Add Phone</Link>
        </Menu.Item>

        <Menu.Item key="/admin/phonelist" icon={<UnorderedListOutlined />}>
          <Link to="/admin/phonelist">Phone List</Link>
        </Menu.Item>

        {isLoggedIn ? (
          <Menu.Item key="logout" icon={<LogoutOutlined />} onClick={handleLogout}>
            Logout
          </Menu.Item>
        ) : (
          <Menu.Item key="login" icon={<LoginOutlined />} onClick={handleLogin}>
            Login
          </Menu.Item>
        )}
      </Menu>
    </Sider>
  );
};

export default AdminSidebar;
