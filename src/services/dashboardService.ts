import { api } from "../services/api";


export const getDashboardStats = async () => {
  const response = await api("/dashboard");
  return response.data;
};
