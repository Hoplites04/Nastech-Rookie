import React, { useEffect, useState } from 'react';
import { Layout, Table, Typography, Button, Popconfirm, Space, message } from 'antd';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';
import AdminSidebar from '../components/AdminSidebar';
import { useNavigate } from 'react-router-dom';
import { getPhoneList, deletePhone } from '../api/phoneApi';

const { Title } = Typography;
const { Content } = Layout;

interface Phone {
  id: number;
  name: string;
  brandName: string;

}

const PhoneList: React.FC = () => {
  const [phones, setPhones] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  const navigate = useNavigate();

  useEffect(() => {
    fetchPhones();
  }, []);

  const fetchPhones = async () => {
    setLoading(true);
    try {
      const res = await getPhoneList();
      setPhones(res.data);
    } catch {
      message.error('Failed to load phones.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deletePhone(id);
      message.success('Phone deleted successfully!');
      fetchPhones(); // Refresh list
    } catch {
      message.error('Failed to delete phone.');
    }
  };

  const handleEdit = (id: number) => {
    navigate(`/admin/phoneedit/${id}`);

  };

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
    },
    {
      title: 'Brand',
      dataIndex: 'brandName',
    },

    {
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: Phone) => (
        <Space>
          <Button
            icon={<EditOutlined />}
            onClick={() => handleEdit(record.id)}
          >
            Edit
          </Button>
          <Popconfirm
            title="Are you sure to delete this phone?"
            onConfirm={() => handleDelete(record.id)}
            okText="Yes"
            cancelText="No"
          >
            <Button danger icon={<DeleteOutlined />}>
              Delete
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <AdminSidebar />
      <Layout>
        <Content style={{ margin: '16px', padding: '2rem' }}>
          <Title level={2}>Phone List</Title>
          <Table
            loading={loading} 
            rowKey="id"
            columns={columns}
            dataSource={phones}
            bordered
            pagination={{ pageSize: 5 }}
          />
        </Content>
      </Layout>
    </Layout>
  );
};

export default PhoneList;
