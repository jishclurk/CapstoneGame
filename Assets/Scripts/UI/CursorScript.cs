using System.Collections;
using System.Collections.Generic;
using LayerDefinitions;
using UnityEngine;

public class CursorScript : MonoBehaviour {

    public Texture2D navTexture;
    public Texture2D shootTexture;
    public Texture2D abilityTexture;
    public Texture2D defendTexture;

    public Texture2D navTextureSmall;
    public Texture2D shootTextureSmall;
    public Texture2D abilityTextureSmall;
    public Texture2D defendTextureSmall;

    private Texture2D navCursor;
    private Texture2D shootCursor;
    private Texture2D abilityCursor;
    private Texture2D defendCursor;
    public CursorMode cursorMode = CursorMode.ForceSoftware;

    private TeamManager tm;

    // Use this for initialization
    void Start () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        if(Screen.height < 899f)
        {
            navCursor = Instantiate(navTextureSmall) as Texture2D;
            shootCursor = Instantiate(shootTextureSmall) as Texture2D;
            abilityCursor = Instantiate(abilityTextureSmall) as Texture2D;
            defendCursor = Instantiate(defendTextureSmall) as Texture2D;
        } else
        {
            navCursor = Instantiate(navTexture) as Texture2D;
            shootCursor = Instantiate(shootTexture) as Texture2D;
            abilityCursor = Instantiate(abilityTexture) as Texture2D;
            defendCursor = Instantiate(defendTexture) as Texture2D;
        }

        Cursor.SetCursor(navCursor, Vector2.zero, cursorMode);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            SquadCursorControl();
        } else
        {
            PlayerCursorControl();
        }




    }

    private void PlayerCursorControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (tm.activePlayer.strategy.playerScript.activeSpecialAbility != null && tm.activePlayer.strategy.playerScript.activeSpecialAbility.GetCoopAction() != AbilityHelper.CoopAction.InstantHeal)
        {
            Cursor.SetCursor(abilityCursor, new Vector2(abilityCursor.height / 2, abilityCursor.width / 2), cursorMode);
        }
        else if (Physics.Raycast(ray, out hit, 100f, Layers.Enemy))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Cursor.SetCursor(shootCursor, new Vector2(shootCursor.height / 2, shootCursor.width / 2), cursorMode);
            }
            /*else
            {
                Cursor.SetCursor(navCursor, Vector2.zero, cursorMode);
            }*/
        }
        else
        {
            Cursor.SetCursor(navCursor, Vector2.zero, cursorMode);
        }
    }

    private void SquadCursorControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (tm.activePlayer.strategy.playerScript.activeSpecialAbility != null && tm.activePlayer.strategy.playerScript.activeSpecialAbility.GetCoopAction() != AbilityHelper.CoopAction.InstantHeal)
        {
            Cursor.SetCursor(abilityCursor, new Vector2(abilityCursor.height / 2, abilityCursor.width / 2), cursorMode);
        }
        else if (Physics.Raycast(ray, out hit, 100f, Layers.Enemy))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Cursor.SetCursor(shootCursor, new Vector2(shootCursor.height / 2, shootCursor.width / 2), cursorMode);
            }
            /*else
            {
                Cursor.SetCursor(navCursor, Vector2.zero, cursorMode);
            }*/
        }
        else
        {
            Cursor.SetCursor(defendCursor, new Vector2(shootCursor.height / 2, shootCursor.width / 2), cursorMode);
        }
    }


}
