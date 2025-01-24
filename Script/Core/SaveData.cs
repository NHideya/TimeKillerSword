using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SaveData
{
    public static SaveData Instance;
    //map 
    public HashSet<string> sceneNames;

    //bench
    public string benchSceneName;
    public Vector2 benchPos;

    //player Stuff
    public float playerHealth;
    public float playerMana;
    public Vector2 playerPosition;
    public string lastScene;
    public bool newGame;
    
     public bool walkTutorialDone;
      public bool jumpTutorialDone;

       public bool wallJumpTutorialDone;
    public bool attackTutorialDone;
    public bool chargeAttackTutorialDone;
    public bool guardTutorialDone;
    public bool counterTutorialDone;
    public bool strongAttackTutorialDone;
    public bool bossBeated;
    
    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.bench.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.bench.data"));
             writer.Write("");
            writer.Write("");
            writer.Write("");
        }
        if (sceneNames == null)
        {
            sceneNames = new HashSet<string>();
        }
    }

    public void SaveBench()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.bench.data")))
        {
            writer.Write(benchSceneName);
            writer.Write(benchPos.x);
            writer.Write(benchPos.y);
        }
    }

    public void LoadBench()
    {
        if (File.Exists(Application.persistentDataPath + "/save.bench.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.bench.data")))
            {
                benchSceneName = reader.ReadString();
                benchPos.x = reader.ReadSingle();
                benchPos.y = reader.ReadSingle();
            }
        }
    }
    public string LoadSceneName()
    {
       
        if (File.Exists(Application.persistentDataPath + "/save.bench.data"))
        {
            Debug.Log(Application.persistentDataPath);
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.bench.data")))
            {
               lastScene = reader.ReadString();
               
            }
            return lastScene;
        }
         
        return "Not_Exist";
    }


   

    public void InitializeFlagForNewGame()
    {
        walkTutorialDone = false;
        jumpTutorialDone = false;
        wallJumpTutorialDone = false;
         attackTutorialDone = false;
                chargeAttackTutorialDone = false;
                strongAttackTutorialDone = false;
                guardTutorialDone = false;
                counterTutorialDone = false;
                bossBeated = false;

    }

    

    public void SavePlayerData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.player.data")))
        {
            playerHealth = PlayerController.Instance.Health;
            writer.Write(playerHealth);
            playerMana = PlayerController.Instance.Mana;
            writer.Write(playerMana);
            playerPosition = PlayerController.Instance.transform.position;
            writer.Write(playerPosition.x);
            writer.Write(playerPosition.y);
          

            lastScene = SceneManager.GetActiveScene().name;
            writer.Write(lastScene);
           
           writer.Write(walkTutorialDone);
            writer.Write(jumpTutorialDone);
            writer.Write(wallJumpTutorialDone);
            writer.Write(attackTutorialDone);
            writer.Write(chargeAttackTutorialDone);
            writer.Write(guardTutorialDone);
            writer.Write(counterTutorialDone);
            writer.Write(strongAttackTutorialDone);
            writer.Write(bossBeated);
           

        }
    }
    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.player.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.player.data")))
            {
                playerHealth = reader.ReadInt32();
                playerMana = reader.ReadSingle();
                playerPosition.x = reader.ReadSingle();
                playerPosition.y = reader.ReadSingle();
               
                lastScene = reader.ReadString();

                SceneManager.LoadScene(lastScene);
                PlayerController.Instance.transform.position = playerPosition;
                 
                PlayerController.Instance.Health = playerHealth;
                PlayerController.Instance.Mana = playerMana;
               walkTutorialDone = reader.ReadBoolean();
                jumpTutorialDone = reader.ReadBoolean();
                wallJumpTutorialDone = reader.ReadBoolean();
                attackTutorialDone = reader.ReadBoolean();
                chargeAttackTutorialDone = reader.ReadBoolean();
                guardTutorialDone = reader.ReadBoolean();
                counterTutorialDone = reader.ReadBoolean();
                strongAttackTutorialDone = reader.ReadBoolean();
                bossBeated = reader.ReadBoolean();

            }
        }
        else
        {
            Debug.Log("File doesnt exits");
            PlayerController.Instance.Health = PlayerController.Instance.maxHealth;
            
        }
    }
}
