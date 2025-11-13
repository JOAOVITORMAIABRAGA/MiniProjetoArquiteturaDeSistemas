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

  // campos de busca por ID
  const [filtroDiscente, setFiltroDiscente] = useState("");
  const [filtroDisciplina, setFiltroDisciplina] = useState("");
  const [filtroLivro, setFiltroLivro] = useState("");
  const [filtroMatricula, setFiltroMatricula] = useState("");
  const [filtroReserva, setFiltroReserva] = useState("");

  // campos para ações
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
      setter(Array.isArray(data) ? data : [data]);
    } catch {
      setMensagem("Erro ao consultar API.");
    }
  }

  // ✅ Funções de filtro
  async function filtrarDiscente() {
    const url = filtroDiscente
      ? `${API}/discente/${filtroDiscente}`
      : `${API}/discente`;
    await fetchData(url, setDiscentes);
  }

  async function filtrarDisciplina() {
    const url = filtroDisciplina
      ? `${API}/disciplina/${filtroDisciplina}`
      : `${API}/disciplina`;
    await fetchData(url, setDisciplinas);
  }

  async function filtrarLivro() {
    const url = filtroLivro ? `${API}/biblioteca/${filtroLivro}` : `${API}/biblioteca`;
    await fetchData(url, setLivros);
  }

  async function filtrarMatricula() {
    const url = filtroMatricula
      ? `${API}/matricula/${filtroMatricula}`
      : `${API}/matricula`;
    await fetchData(url, setMatriculas);
  }

async function filtrarReserva() {
  const url = filtroReserva
    ? `${API}/reserva/discente/${filtroReserva}` // ← rota correta
    : `${API}/reserva`; // lista todas as reservas
  await fetchData(url, setReservas);
}


  // ✅ MATRICULAR
  async function matricular() {
    setMensagem("");
    try {
      const response = await fetch(`${API}/matricula/matricular`, {
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
      const response = await fetch(`${API}/reserva/reservar`, {
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

  // ✅ DESMATRICULAR
  async function desmatricular(discenteId: number, disciplinaId: number) {
    await fetch(`${API}/matricula/cancelar/${discenteId}/${disciplinaId}`, {
      method: "DELETE",
    });
    atualizarTudo();
  }

  // ✅ DEVOLVER LIVRO
  async function devolver(discenteId: number, livroId: number) {
    await fetch(`${API}/reserva/cancelar/${discenteId}/${livroId}`, {
      method: "DELETE",
    });
    atualizarTudo();
  }

  // ✅ ALTERAR STATUS
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

      {/* FILTROS E TABELAS */}
      <FiltroTabela
        titulo="Discentes"
        valor={filtroDiscente}
        onChange={setFiltroDiscente}
        onFiltrar={filtrarDiscente}
      />
      <RenderTable
        title="Discentes"
        data={discentes}
        actions={(discente) => (
          <DiscenteActions discente={discente} alterarStatus={alterarStatus} />
        )}
      />

      <FiltroTabela
        titulo="Disciplinas"
        valor={filtroDisciplina}
        onChange={setFiltroDisciplina}
        onFiltrar={filtrarDisciplina}
      />
      <RenderTable title="Disciplinas" data={disciplinas} />

      <FiltroTabela
        titulo="Livros"
        valor={filtroLivro}
        onChange={setFiltroLivro}
        onFiltrar={filtrarLivro}
      />
      <RenderTable title="Livros" data={livros} />

      <FiltroTabela
        titulo="Matrículas"
        valor={filtroMatricula}
        onChange={setFiltroMatricula}
        onFiltrar={filtrarMatricula}
      />
      <RenderTable
        title="Matrículas"
        data={matriculas}
        actions={(item) => (
          <button onClick={() => desmatricular(item.discenteId, item.disciplinaId)}>
            ❌ Desmatricular
          </button>
        )}
      />

      <FiltroTabela
        titulo="Reservas"
        valor={filtroReserva}
        onChange={setFiltroReserva}
        onFiltrar={filtrarReserva}
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
   COMPONENTE: Filtro por ID
================================================================ */
interface FiltroTabelaProps {
  titulo: string;
  valor: string;
  onChange: (v: string) => void;
  onFiltrar: () => void;
}

function FiltroTabela({ titulo, valor, onChange, onFiltrar }: FiltroTabelaProps) {
  return (
    <div style={{ marginTop: "25px" }}>
      <h2>{titulo}</h2>
      <div style={{ display: "flex", gap: "10px", alignItems: "center" }}>
        <input
          placeholder={`Filtrar ${titulo} por ID`}
          value={valor}
          onChange={(e) => onChange(e.target.value)}
        />
        <button onClick={onFiltrar}>Filtrar</button>
      </div>
    </div>
  );
}

/* ================================================================
   COMPONENTE: Tabela
================================================================ */
interface RenderTableProps {
  title: string;
  data: any[];
  actions?: (item: any) => ReactNode;
}

function RenderTable({ title, data, actions }: RenderTableProps) {
  if (!data || data.length === 0) return null;

  return (
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
            <TableRow key={index} item={item} actions={actions} />
          ))}
        </tbody>
      </table>
    </div>
  );
}

function TableRow({ item, actions }: { item: any; actions?: (item: any) => ReactNode }) {
  return (
    <tr>
      {Object.values(item).map((value, idx) => (
        <td key={idx} style={{ border: "1px solid black", padding: "5px" }}>
          {typeof value === "object" && value !== null ? JSON.stringify(value) : value?.toString()}
        </td>
      ))}
      {actions ? <td>{actions(item)}</td> : null}
    </tr>
  );
}

/* ================================================================
   COMPONENTE: Ações dos Discentes
================================================================ */
function DiscenteActions({
  discente,
  alterarStatus,
}: {
  discente: any;
  alterarStatus: (id: number, status: string) => void;
}) {
  const [novoStatus, setNovoStatus] = useState(discente.status || "");

  return (
    <div style={{ display: "flex", gap: "5px", alignItems: "center" }}>
      <select value={novoStatus} onChange={(e) => setNovoStatus(e.target.value)}>
        <option value="">Selecione...</option>
        <option value="Ativo">Ativo</option>
        <option value="Trancado">Trancado</option>
        <option value="Concluído">Concluído</option>
      </select>
      <button onClick={() => alterarStatus(discente.id, novoStatus)}>Alterar</button>
    </div>
  );
}
