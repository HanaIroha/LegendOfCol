using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static AudioClip MonsterHit, MonsterDie, PlayerAttack, PlayerJump, PlayerGetHit, PlayerPowerUp, PlayerDie, GoblinAttack, DeflectSound, HeartEat,
        PlayerJumpLand, PlayerBlock, PlayerDeflect, BossAttackAlert, CheckPoint, Teleport, Spawn;
    static AudioSource audioScr;
    // Start is called before the first frame update
    void Start()
    {
        MonsterHit = Resources.Load<AudioClip>("MonsterHit1");
        MonsterDie = Resources.Load<AudioClip>("MonsterDie1");
        PlayerAttack = Resources.Load<AudioClip>("PlayerAttack");
        PlayerJump = Resources.Load<AudioClip>("PlayerJump");
        PlayerDie = Resources.Load<AudioClip>("PlayerDie");
        GoblinAttack = Resources.Load<AudioClip>("GoblinAttack");
        DeflectSound = Resources.Load<AudioClip>("DeflectSound");
        HeartEat = Resources.Load<AudioClip>("HeartEat");
        PlayerJumpLand = Resources.Load<AudioClip>("PlayerJumpLand");
        PlayerBlock = Resources.Load<AudioClip>("PlayerBlock");
        PlayerDeflect = Resources.Load<AudioClip>("PlayerDeflectz");
        BossAttackAlert = Resources.Load<AudioClip>("BossAttackAlert");
        PlayerGetHit = Resources.Load<AudioClip>("PlayerGetHit");
        PlayerPowerUp = Resources.Load<AudioClip>("PlayerPowerUp");
        CheckPoint = Resources.Load<AudioClip>("CheckPoint");
        Teleport = Resources.Load<AudioClip>("Teleport");
        Spawn = Resources.Load<AudioClip>("Spawn");
        audioScr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void playSound (string clip)
    {
        switch (clip)
        {
            case "MonsterHit":
                audioScr.PlayOneShot(MonsterHit);
                break;
            case "MonsterDie":
                audioScr.PlayOneShot(MonsterDie);
                break;
            case "PlayerAttack":
                audioScr.PlayOneShot(PlayerAttack);
                break;
            case "PlayerJump":
                audioScr.PlayOneShot(PlayerJump);
                break;
            case "PlayerDie":
                audioScr.PlayOneShot(PlayerDie);
                break;
            case "GoblinAttack":
                audioScr.PlayOneShot(GoblinAttack);
                break;
            case "DeflectSound":
                audioScr.PlayOneShot(DeflectSound);
                break;
            case "HeartEat":
                audioScr.PlayOneShot(HeartEat);
                break;
            case "PlayerJumpLand":
                audioScr.PlayOneShot(PlayerJumpLand);
                break;
            case "PlayerBlock":
                audioScr.PlayOneShot(PlayerBlock);
                break;
            case "PlayerDeflect":
                audioScr.PlayOneShot(PlayerDeflect);
                break;
            case "BossAttackAlert":
                audioScr.PlayOneShot(BossAttackAlert);
                break;
            case "PlayerGetHit":
                audioScr.PlayOneShot(PlayerGetHit);
                break;
            case "PlayerPowerUp":
                audioScr.PlayOneShot(PlayerPowerUp);
                break;
            case "CheckPoint":
                audioScr.PlayOneShot(CheckPoint);
                break;
            case "Teleport":
                audioScr.PlayOneShot(Teleport);
                break;
            case "Spawn":
                audioScr.PlayOneShot(Spawn);
                break;
        }
    }
}
