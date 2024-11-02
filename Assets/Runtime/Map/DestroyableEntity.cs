using Character;
using EditorExtend.GridEditor;

public class DestroyableEntity : CharacterEntity
{
    [AutoComponent]
    public GridObject GridObject { get; private set; }


}
