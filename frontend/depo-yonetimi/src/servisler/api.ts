import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5143/api',
  headers: { 'Content-Type': 'application/json' },
});

// Backend PascalCase döndürür, frontend camelCase'e dönüştürür
api.interceptors.response.use(
  (response) => {
    response.data = toCamel(response.data);
    return response;
  },
  (error) => Promise.reject(error)
);

// ─── PascalCase → camelCase dönüşüm yardımcısı ───────────────────────────────
function toCamelKey(key: string): string {
  return key.charAt(0).toLowerCase() + key.slice(1);
}

function toCamel(obj: unknown): unknown {
  if (Array.isArray(obj)) return obj.map(toCamel);
  if (obj !== null && typeof obj === 'object') {
    const result: Record<string, unknown> = {};
    for (const k in obj as Record<string, unknown>) {
      result[toCamelKey(k)] = toCamel((obj as Record<string, unknown>)[k]);
    }
    return result;
  }
  return obj;
}

export default api;
