import React, { useEffect, useState } from 'react';
import { Layout, Form, Input, InputNumber, Select, Button, Typography, message, Spin } from 'antd';
import { useParams, useNavigate } from 'react-router-dom';
import AdminSidebar from '../components/AdminSidebar';
import { updatePhone, getPhoneById } from '../api/phoneApi'; // Uncomment when ready

const { Title } = Typography;
const { Option } = Select;
const { Content } = Layout;

const PhoneEdit: React.FC = () => {
  const [form] = Form.useForm();
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [brands, setBrands] = useState<string[]>([]);


  useEffect(() => {
    // Load brands (mocked for now)
    setBrands(['Apple', 'Samsung', 'Xiaomi', 'Oppo', 'Vivo']);

    // Load phone by ID (mocked data for now)
    const fetchPhone = async () => {
      try {
        const res = await getPhoneById(Number(id));
        const phone = res.data;

        form.setFieldsValue({
          name: phone.name,
          phoneBrandId: phone.phoneBrandId, // use correct backend field
          description: phone.description,
        });
      } catch (error) {
        message.error('Failed to load phone data.');
      } finally {
        setLoading(false);
      }
    };

    fetchPhone();
  }, [id, form]);

  const onFinish = async (values: any) => {
    try {
      await updatePhone(Number(id), values); // sends PUT
      message.success('Phone updated successfully!');
      navigate('/admin/phonelist');
    } catch (error) {
      message.error('Failed to update phone.');
    }
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <AdminSidebar />
      <Layout>
        <Content style={{ margin: '16px', display: 'flex', justifyContent: 'center' }}>
          <div style={{ maxWidth: 900, width: '100%', padding: '2rem' }}>
            <Title level={2}>Edit Phone</Title>
            {loading ? (
              <Spin />
            ) : (
              <Form form={form} layout="vertical" onFinish={onFinish}>
                <Form.Item
                  name="name"
                  label="Phone Name"
                  rules={[{ required: true, message: 'Please input the phone name!' }]}
                >
                  <Input />
                </Form.Item>

                <Form.Item
                  name="phoneBrandId"
                  label="Brand"
                  rules={[{ required: true, message: 'Please select a brand!' }]}
                >
                  <Select>
                    {brands.map((brand, index) => (
                      <Option key={index + 1} value={index + 1}>
                        {brand}
                      </Option>
                    ))}
                  </Select>
                </Form.Item>


                <Form.Item
                  name="description"
                  label="Description"
                  rules={[{ required: true, message: 'Please input a description!' }]}
                >
                  <Input.TextArea rows={4} />
                </Form.Item>

                <Form.Item>
                  <Button type="primary" htmlType="submit">
                    Update Phone
                  </Button>
                </Form.Item>
              </Form>
            )}
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default PhoneEdit;
