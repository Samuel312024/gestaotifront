import { api } from "./api";

export interface Usuario {
  id: number;
  nome: string;
  email: string;
  role: string;
  ativo: boolean;
}

export interface FormUsuario {
  nome: string;
  email: string;
  senha: string;
  role: string;
}

export async function listarUsuarios(): Promise<Usuario[]> {
  return api("/api/usuario"); // GET
}

export async function criarUsuario(dados: FormUsuario) {
  return api("/api/usuario", {
    method: "POST",
    body: JSON.stringify(dados),
  });
}

export async function listarRoles(): Promise<string[]> {
  const response = await fetch("https://localhost:5173/api/usuario/roles");

  if (!response.ok) {
    throw new Error("Erro ao carregar roles");
  }

  return response.json();
}



export async function editarUsuario(id: number, dados: Partial<Usuario>) {
  return api(`/api/usuario/${id}`, {
    method: "PUT",
    body: JSON.stringify(dados),
  });
}
