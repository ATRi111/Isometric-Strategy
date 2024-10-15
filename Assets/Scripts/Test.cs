using UnityEngine;

public class Test : MonoBehaviour
{
    public Grid grid;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.Set(world.x, world.y, 0);
            Vector3Int cell = grid.WorldToCell(world);
            world = grid.CellToWorld(cell);
            transform.position = world + new Vector3(0f, 0.5f * grid.cellSize.y, 0f);
        }
    }
}