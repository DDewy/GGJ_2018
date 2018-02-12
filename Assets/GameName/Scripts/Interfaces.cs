interface Interactable
{
    void Clicked(bool Leftclick);
}

public interface ITileHit
{
    void TileHit(UnityEngine.Vector2Int HitDirection, TileColor lightColor); //We could optimise this by changing Colour into a Byte since we only need a few colour variations
}