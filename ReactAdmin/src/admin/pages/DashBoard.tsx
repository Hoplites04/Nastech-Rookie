import React from 'react';
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Layout, Menu, Card, Typography, Button } from 'antd';
import AdminSidebar from '../components/AdminSidebar'; // Adjust the import path as necessary
const { Header, Sider, Content } = Layout;
const { Title, Text } = Typography;
import userManager from '../../auth/authConfig'; // adjust path if needed

const DashBoard: React.FC = () => {

    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

  useEffect(() => {
    userManager.getUser().then(user => {
      setIsAuthenticated(!!user && !user.expired);
    });
  }, []);


    return (
        <Layout style={{ minHeight: '100vh' }}>
        <AdminSidebar />
  
        <Layout>
          <Content
            style={{
              margin: '16px',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              minHeight: 'calc(100vh - 64px)',
            }}
          >
            {isAuthenticated === null ? (
              <Text>Loading...</Text>
            ) : !isAuthenticated ? (
              <>
                <Title level={2}>Please login to access admin dashboard</Title>
              </>
            ) : (
              <>
                <Title level={2}>Welcome, Admin</Title>
                <Text>Here’s an overview of your platform:</Text>
  
                <div style={{ display: 'flex', gap: 16, marginTop: 24 }}>
                  <Card title="Total Phones" bordered style={{ width: 240 }}>
                    <Text strong>128</Text>
                  </Card>
                  <Card title="Brands" bordered style={{ width: 240 }}>
                    <Text strong>12</Text>
                  </Card>
                  <Card title="Pending Orders" bordered style={{ width: 240 }}>
                    <Text strong>5</Text>
                  </Card>
                </div>
              </>
            )}
          </Content>
        </Layout>
      </Layout>
    );
};

export default DashBoard;
