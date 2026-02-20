import { api } from "./api";

export async function getStats() {
  return await api("/api/chamados/stats");
}

export async function listarChamados() {
  return await api("/api/chamados");
}

export async function meusChamados() {
  return await api("/api/chamados/meus");
}

export async function abrirChamado(data: any) {
  return await api("/api/chamados", {
    method: "POST",
    body: JSON.stringify(data),
  });
}

export async function assumirChamado(id: number) {
  return await api(`/api/chamados/${id}/assumir`, {
    method: "PUT",
  });
}

export async function fecharChamado(id: number) {
  return await api(`/api/chamados/${id}/fechar`, {
    method: "PUT",
  });
}
