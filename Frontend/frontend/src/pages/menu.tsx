import { useState } from "react";

interface GenericObject {
  [key: string]: any;
}

export default function ConsultaApi() {
  const [resultado, setResultado] = useState<GenericObject[]>([]);
  const [loading, setLoading] = useState(false);

  const backendUrl = "https://localhost:7005"; // altere se necessário

  async function buscar(endpoint: string) {
    setLoading(true);
    setResultado([]);

    try {
      const res = await fetch(`${backendUrl}/api/${endpoint}`);

      if (!res.ok) {
        throw new Error(`Erro na requisição: ${res.status}`);
      }

      const dados: GenericObject[] = await res.json();
      setResultado(dados);
    } catch (error: any) {
      alert(error.message ?? "Erro inesperado.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{ padding: 20, fontFamily: "Arial" }}>
      <h1>Consulta ao Backend (.NET API)</h1>

      <div style={{ display: "flex", gap: 10, marginBottom: 20 }}>
        <button onClick={() => buscar("discente")}>Consultar Discentes</button>
        <button onClick={() => buscar("disciplina")}>Consultar Disciplinas</button>
        <button onClick={() => buscar("biblioteca")}>Consultar Livros</button>
      </div>

      {loading && <p>Carregando...</p>}

      {!loading && resultado.length === 0 && <p>Nenhum dado carregado.</p>}

      {!loading && resultado.length > 0 && (
        <table border={1} cellPadding={10}>
          <thead>
            <tr>
              {Object.keys(resultado[0]).map((col: string) => (
                <th key={col}>{col}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {resultado.map((item: GenericObject, idx: number) => (
              <tr key={idx}>
                {Object.keys(item).map((col: string) => (
                  <td key={col}>{item[col]}</td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
