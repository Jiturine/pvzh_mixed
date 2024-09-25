using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectCardSceneManager : MonoBehaviour
{
    public Button plantHeroButton;
    public Button zombieHeroButton;
    public Button startGameButton;
    public Button getReadyButton;
    public Image plantHeroIcon;
    public Image zombieHeroIcon;
    // Start is called before the first frame update
    void Start()
    {
        plantHeroButton = GameObject.Find("Plant Hero Icon").GetComponent<Button>();
        zombieHeroButton = GameObject.Find("Zombie Hero Icon").GetComponent<Button>();
        getReadyButton = GameObject.Find("Get Ready").GetComponent<Button>();
        startGameButton = GameObject.Find("Start Game").GetComponent<Button>();
        plantHeroIcon = GameObject.Find("Plant Hero Icon").GetComponent<Image>();
        zombieHeroIcon = GameObject.Find("Zombie Hero Icon").GetComponent<Image>();
        plantHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        zombieHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        getReadyButton.onClick.AddListener(GameManager.Instance.OnGetReadyBtnClick);
        startGameButton.onClick.AddListener(GameManager.Instance.OnStartGameBtnClick);

        GameManager.Instance.LoadSelectCardScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.myHeroFaction == GameManager.Faction.Plant)
        {
            plantHeroIcon.color = Color.white;
            zombieHeroIcon.color = Color.grey;
        }
        else
        {
            plantHeroIcon.color = Color.grey;
            zombieHeroIcon.color = Color.white;
        }
    }
}
