const BASE_URL = "https://localhost:44367";

export async function api(url: string, options: any = {}) {
  const token = localStorage.getItem("token");

  const response = await fetch(`${BASE_URL}${url}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token && { Authorization: `Bearer ${token}` }),
      ...options.headers,
    },
  });

  // 🔥 Se token expirou
  if (response.status === 401) {
    localStorage.removeItem("token");
    window.location.href = "/login";
    return;
  }

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Erro na requisição");
  }

  return response.json();
}
