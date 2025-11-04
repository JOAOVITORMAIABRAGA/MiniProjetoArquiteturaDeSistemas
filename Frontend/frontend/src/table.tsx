// ✅ Importação corretamente tipada (exigida quando 'verbatimModuleSyntax' está ativo)
import type { ReactNode } from "react";

interface TableProps {
  headers: string[];
  children: ReactNode;
}

export default function Table({ headers, children }: TableProps) {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse" }}>
      <thead>
        <tr>
          {headers.map((header) => (
            <th
              key={header}
              style={{
                padding: "10px",
                borderBottom: "2px solid #ccc",
                textAlign: "left",
              }}
            >
              {header}
            </th>
          ))}
        </tr>
      </thead>

      <tbody>{children}</tbody>
    </table>
  );
}
