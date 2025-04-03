import { createEffect, createSignal, For } from 'solid-js'

type TableItem = {
  id: number
  value: string
}

type Table = {
  items: TableItem[]
  title: string
}

const fetchTable = async (id: number) => 
  (await fetch(`/api/table/${id}`)).json();
const createTableItem = async (tableId: number) => 
  (await fetch(`/api/table/${tableId}/items`, {method:"POST"})); 
const updateTableItem = async (tableItemId: number, value: string) =>
  (await fetch(`/api/table/items/${tableItemId}?value=${value}`, {method:"PUT"}));
const deleteTableItem = async (tableItemId: number) =>
  (await fetch(`/api/table/items/${tableItemId}`, {method:"DELETE"}));

function Table() {
  const [tableId, setActiveTableId] = createSignal<number>(1);
  const [table, setTable] = createSignal<Table>();
  const [error, setError] = createSignal<boolean>(false)
  const [loading, setLoading] = createSignal<boolean>(false)

  createEffect(async () => {
    try {
      setError(false);
      setLoading(true);
      const newTable = await fetchTable(tableId());
      setTable(newTable);
      setLoading(false);
    } catch {
      setError(true);
    }
  })

  const handleCreateTableItem = async () => {
    try {
      setError(false);
      await createTableItem(tableId());
      const newTable = await fetchTable(tableId());
      setTable(newTable);
    } catch {
      setError(true);
    }
  }
  const handleUpdateTableItem = async (tableItemId: number, value: string) => {
    try {
      setError(false);
      await updateTableItem(tableItemId, value);
      const newTable = await fetchTable(tableId());
      setTable(newTable);
    } catch {
      setError(true);
    }
  }
  const handleDeleteTableItem = async (tableItemId: number) => {
    try {
      setError(false);
      await deleteTableItem(tableItemId);
      const newTable = await fetchTable(tableId());
      setTable(newTable);
    } catch {
      setError(true);
    }
  }

  return (<>
    <div>
      <h2>title: {table()?.title ?? 'No title available'}</h2>
      <h3>id: {tableId()}</h3>
    </div>

    {/* Navigation */}
    <div>
      <button onclick={() => setActiveTableId(prev => prev - 1)}>-</button>
      <button onclick={() => setActiveTableId(prev => prev + 1)}>+</button>
    </div>

    {table() && <>
      <table>
        <thead>
          <tr>
            <th>id</th>
            <th>Value</th>
          </tr>
        </thead>

        {/* Items */}
        <tbody>
          <For each={table()?.items}>{ item =>
            <tr>
              <td>{item.id}</td>
              <td>
                <textarea 
                  value={item.value}
                  onChange={(e) => handleUpdateTableItem(item.id, e.target.value)} 
                >
                  {item.value}
                </textarea>
              </td>
              <td><button onclick={() => handleDeleteTableItem(item.id)}> - </button></td>
            </tr>
          }</For>
          <tr><td colSpan={3}><button onclick={handleCreateTableItem}> + </button></td></tr>
        </tbody>
      </table>
    </>}
  </>);
}

export default Table
