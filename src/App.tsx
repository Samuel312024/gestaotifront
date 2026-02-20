import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import CreateUser from "./pages/CreateUser";
import Dashboard from "./pages/Dashboard";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import Register from "./pages/Register";
import NovoChamado from "./pages/NovoChamado";
import ListaChamados from "./pages/ListaChamados";
import Layout from "./components/Layout";
import ProtectedRoute from "./routes/ProtectedRoute";
import Usuarios from "./pages/Usuarios";
import NovoUsuario from "./pages/Usuarios";
import EditarUsuario from "./pages/Usuarios";

function App() {
  return (
    <Routes>
      {/* ================= ROTAS PÚBLICAS ================= */}
      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/forgot-password" element={<ForgotPassword />} />
      <Route path="/reset-password" element={<ResetPassword />} />

      {/* ================= ROTAS PROTEGIDAS ================= */}
      <Route
        element={
          <ProtectedRoute>
            <Layout />
          </ProtectedRoute>
        }
      >
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/chamados" element={<ListaChamados />} />
        <Route path="/chamados/novo" element={<NovoChamado />} />
        <Route path="/create-user" element={<CreateUser />} />
        <Route path="/usuarios" element={<Usuarios />} />
        <Route path="/usuarios/novo" element={<NovoUsuario />} />
        <Route path="/usuarios/editar/:id" element={<EditarUsuario />} />

      </Route>
    </Routes>
  );
}

export default App;
