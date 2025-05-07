import React, { useState, useEffect } from 'react';
import { Layout, Form, Input, Select, Button, Typography, message } from 'antd';
import { createPhone } from '../api/phoneApi';
import AdminSidebar from '../components/AdminSidebar';
import { Content } from 'antd/es/layout/layout';

const { Title } = Typography;
const { Option } = Select;

const PhoneAdd: React.FC = () => {
  const [form] = Form.useForm();
  const [loadingBrands, setLoadingBrands] = useState<boolean>(false);
  const [brandOptions, setBrandOptions] = useState<{ label: string; value: number }[]>([]);

  useEffect(() => {
    // Simulate API delay
    setLoadingBrands(true);

    const brands = [
      { label: 'Apple', value: 1 },
      { label: 'Samsung', value: 2 },
      { label: 'Xiaomi', value: 3 },
      { label: 'OnePlus', value: 4 },
    ];

    setTimeout(() => {
      setBrandOptions(brands);
      setLoadingBrands(false);
    }, 500);
  }, []);

  const handleSubmit = async (values: any) => {
    try {
      await createPhone(values); // { name, description, phoneBrandId }
      console.log('Phone data submitted:', values);
      message.success('Phone added successfully!');
      form.resetFields();
    } catch (error) {
      message.error('Failed to add phone.');
    }
  };

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
          <div style={{ maxWidth: 900, width: '100%', padding: '2rem' }}>
            <Title level={2}>Add New Phone</Title>

            <Form
              form={form}
              layout="vertical"
              onFinish={handleSubmit}
              initialValues={{}}
            >
              <Form.Item
                name="name"
                label="Phone Name"
                rules={[{ required: true, message: 'Please input the phone name!' }]}
              >
                <Input placeholder="e.g., iPhone 16 Pro Max" />
              </Form.Item>

              <Form.Item
                name="phoneBrandId"
                label="Brand"
                rules={[{ required: true, message: 'Please select a brand!' }]}
              >
                <Select placeholder="Select a brand" loading={loadingBrands}>
                  {brandOptions.map((brand) => (
                    <Option key={brand.value} value={brand.value}>
                      {brand.label}
                    </Option>
                  ))}
                </Select>
              </Form.Item>

              <Form.Item
                name="description"
                label="Description"
                rules={[{ required: true, message: 'Please input a description!' }]}
              >
                <Input.TextArea rows={4} placeholder="Write about the phone's features..." />
              </Form.Item>

              <Form.Item>
                <Button type="primary" htmlType="submit">
                  Add Phone
                </Button>
              </Form.Item>
            </Form>
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default PhoneAdd;
