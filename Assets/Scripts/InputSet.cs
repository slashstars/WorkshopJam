using System;

public class InputSet
{
    public readonly string horizontalAxis = "Horizontal_";
    public readonly string fire = "Fire_";
    public readonly string altFire = "AltFire_";
    public readonly string jump = "Jump_";

    public InputSet(PlayerID id)
    {
        string playerSuffix = "P1";
        if (id == PlayerID.P2)
            playerSuffix = "P2";

        horizontalAxis = "Horizontal_" + playerSuffix;
        fire = "Fire_" + playerSuffix;
        altFire = "AltFire_" + playerSuffix;
        jump = "Jump_" + playerSuffix;
    }
}
