using UnityEngine;

/// <summary>
/// Attach to a place to make sure that player can
/// go there only once
/// </summary>
public class AlreadyWasHere : MonoBehaviour
{
    public bool wasHere { get; private set; }

    private void Start()
    {
        wasHere = false;
    }
    /// <summary>
    /// call the first time the player goes there
    /// </summary>
    public void PlayerWasHere()
   {
        wasHere = true;
   }
   
}
