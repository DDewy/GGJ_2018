interface Interactable
{
    void Clicked(bool Leftclick);
}

public interface ITileHit
{
    void TileHit(UnityEngine.Vector2Int HitDirection, TileColor lightColor); //We could optimise this by changing Colour into a Byte since we only need a few colour variations
}

public interface ITriggerable
{
    void Activate();
    void Deactivate();
}

public interface IEmitsLight
{
    void EmitLight();
    void RefreshFromPoint(UnityEngine.Vector2Int refreshPoint);
}