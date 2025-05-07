
import instance from './axios'; // or '../axiosInstance' depending on your path

export const getPhoneList = () => {
    return instance.get('https://localhost:7251/api/Phone');
  };
  
export const createPhone = (data: any) => {
    return instance.post('https://localhost:7251/api/Phone', data);
  };
  
  // ✅ Update a phone (replace with correct endpoint and method later)
  export const updatePhone = (id: number, data: any) => {
    return instance.put(`https://localhost:7251/api/Phone/${id}`, data);
  };
  
  // ✅ Get phone by ID
  export const getPhoneById = (id: number) => {
    return instance.get(`https://localhost:7251/api/Phone/${id}`);
  };

  export const deletePhone = (id: number) => {
    return instance.delete(`/api/Phone/${id}`);
  };

