import { useEffect, useState } from "react";
import type { ReactNode } from "react";

const API = "https://localhost:7005/api";

export default function Home() {
  const [discentes, setDiscentes] = useState<any[]>([]);
  const [disciplinas, setDisciplinas] = useState<any[]>([]);
  const [livros, setLivros] = useState<any[]>([]);
  const [matriculas, setMatriculas] = useState<any[]>([]);
  const [reservas, setReservas] = useState<any[]>([]);

  const [mensagem, setMensagem] = useState("");

  const [discenteIdMatricula, setDiscenteIdMatricula] = useState("");
  const [disciplinaIdMatricula, setDisciplinaIdMatricula] = useState("");

  const [discenteIdReserva, setDiscenteIdReserva] = useState("");
  const [livroIdReserva, setLivroIdReserva] = useState("");

  async function atualizarTudo() {
    await fetchData(`${API}/discente`, setDiscentes);
    await fetchData(`${API}/disciplina`, setDisciplinas);
    await fetchData(`${API}/biblioteca`, setLivros);
    await fetchData(`${API}/matricula`, setMatriculas);
    await fetchData(`${API}/reserva`, setReservas);
  }

  useEffect(() => {
    atualizarTudo();
  }, []);

  async function fetchData(url: string, setter: (data: any[]) => void) {
    try {
      const response = await fetch(url);
      if (!response.ok) throw new Error("Erro ao buscar dados.");
      const data = await response.json();
      setter(data);
    } catch {
      setMensagem("Erro ao consultar API.");
    }
  }

  // ✅ MATRICULAR
  async function matricular() {
    setMensagem("");
    try {
      const response = await fetch(`${API}/Matricula/matricular`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          discenteId: Number(discenteIdMatricula),
          disciplinaId: Number(disciplinaIdMatricula),
        }),
      });

      const result = await response.text();
      setMensagem(result);
      atualizarTudo();
    } catch {
      setMensagem("Erro ao tentar matricular.");
    }
  }

  // ✅ RESERVAR LIVRO
  async function reservarLivro() {
    setMensagem("");
    try {
      const response = await fetch(`${API}/Reserva/reservar`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          discenteId: Number(discenteIdReserva),
          livroId: Number(livroIdReserva),
        }),
      });

      const result = await response.text();
      setMensagem(result);
      atualizarTudo();
    } catch {
      setMensagem("Erro ao tentar reservar livro.");
    }
  }

  // ✅ DESMATRICULAR — agora passando DiscenteId e DisciplinaId na URL
  async function desmatricular(discenteId: number, disciplinaId: number) {
    await fetch(`${API}/Matricula/cancelar/${discenteId}/${disciplinaId}`, {
      method: "DELETE",
    });

    atualizarTudo();
  }

  // ✅ DEVOLVER LIVRO — agora passando DiscenteId e LivroId na URL
  async function devolver(discenteId: number, livroId: number) {
    await fetch(`${API}/Reserva/cancelar/${discenteId}/${livroId}`, {
      method: "DELETE",
    });

    atualizarTudo();
  }

  // ✅ ALTERAR STATUS DO ALUNO
  async function alterarStatus(id: number, status: string) {
    setMensagem("");
    try {
      const response = await fetch(`${API}/discente/${id}/status`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(status),
      });

      const result = await response.text();
      setMensagem(result);
      atualizarTudo();
    } catch {
      setMensagem("Erro ao alterar status.");
    }
  }

  return (
    <div style={{ padding: "20px", fontFamily: "Arial", maxWidth: "1200px" }}>
      <h1>Sistema Acadêmico + Biblioteca</h1>

      <p style={{ color: "green", fontWeight: "bold" }}>{mensagem}</p>

      <hr />

      {/* MATRICULAR */}
      <h3>Matricular Discente → Disciplina</h3>
      <input
        placeholder="ID do Discente"
        value={discenteIdMatricula}
        onChange={(e) => setDiscenteIdMatricula(e.target.value)}
      />
      <input
        placeholder="ID da Disciplina"
        value={disciplinaIdMatricula}
        onChange={(e) => setDisciplinaIdMatricula(e.target.value)}
      />
      <button onClick={matricular}>Matricular</button>

      {/* RESERVAR LIVRO */}
      <h3>Emprestar Livro → Discente</h3>
      <input
        placeholder="ID do Discente"
        value={discenteIdReserva}
        onChange={(e) => setDiscenteIdReserva(e.target.value)}
      />
      <input
        placeholder="ID do Livro"
        value={livroIdReserva}
        onChange={(e) => setLivroIdReserva(e.target.value)}
      />
      <button onClick={reservarLivro}>Emprestar</button>

      {/* TABELA COM STATUS DROPDOWN */}
      <RenderTable
        title="Discentes"
        data={discentes}
        actions={(discente) => {
          const [novoStatus, setNovoStatus] = useState(discente.status || "");

          return (
            <div style={{ display: "flex", gap: "5px", alignItems: "center" }}>
              <select
                value={novoStatus}
                onChange={(e) => setNovoStatus(e.target.value)}
              >
                <option value="">Selecione...</option>
                <option value="Ativo">Ativo</option>
                <option value="Trancado">Trancado</option>
                <option value="Concluído">Concluído</option>
              </select>
              <button onClick={() => alterarStatus(discente.id, novoStatus)}>
                Alterar
              </button>
            </div>
          );
        }}
      />

      <RenderTable title="Disciplinas" data={disciplinas} />

      <RenderTable title="Livros" data={livros} />

      <RenderTable
        title="Matrículas"
        data={matriculas}
        actions={(item) => (
          <button onClick={() => desmatricular(item.discenteId, item.disciplinaId)}>
            ❌ Desmatricular
          </button>
        )}
      />

      <RenderTable
        title="Reservas"
        data={reservas}
        actions={(item) => (
          <button onClick={() => devolver(item.discenteId, item.livroId)}>
            🔄 Devolver
          </button>
        )}
      />
    </div>
  );
}

/* ================================================================
      COMPONENTE TABELA
================================================================ */
interface RenderTableProps {
  title: string;
  data: any[];
  actions?: (item: any) => ReactNode;
}

function RenderTable({ title, data, actions }: RenderTableProps) {
  if (!data || data.length === 0) return null;

  return (
    <div style={{ marginTop: "25px" }}>
      <h2>{title}</h2>
      <div style={{ maxHeight: "250px", overflowY: "auto", border: "1px solid gray" }}>
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr>
              {Object.keys(data[0]).map((key) => (
                <th key={key} style={{ border: "1px solid black", padding: "5px" }}>
                  {key}
                </th>
              ))}
              {actions ? <th>Ações</th> : null}
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={index}>
                {Object.values(item).map((value, idx) => (
                  <td key={idx} style={{ border: "1px solid black", padding: "5px" }}>
                    {typeof value === "object" && value !== null
                      ? JSON.stringify(value)
                      : value?.toString()}
                  </td>
                ))}
                {actions ? <td>{actions(item)}</td> : null}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
