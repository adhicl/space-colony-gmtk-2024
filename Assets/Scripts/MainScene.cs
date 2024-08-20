using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Zenject.Asteroids;

public class MainScene : MonoBehaviour
{
    public SO_ServerData mData;

	[Inject]
	public void Construct()
	{
	}

    public float m_CurrentCash = 1000;
    private int m_CurrentDay = 0;
    private float m_tick = 0f;

    public float timeMultiplier = 1f;

    public List<GameObject> mChildren = new List<GameObject>();
	public List<GameObject> mEnemy = new List<GameObject>();

	[SerializeField] TextMeshProUGUI m_Cash;
    [SerializeField] TextMeshProUGUI m_Day;
	[SerializeField] Image m_HourTick;

	[SerializeField] GameObject p_Upgrade;
	[SerializeField] GameObject p_AddNode;
	[SerializeField] GameObject p_Learn;

	[SerializeField] AudioClip sfxLaser;
	[SerializeField] AudioClip sfxUpgrade;
	[SerializeField] AudioClip sfxBuild;
	[SerializeField] AudioClip sfxExplosion;
	[SerializeField] AudioClip sfxError;
	[SerializeField] AudioClip sfxButton;

	public oServer curServer;

    public int maxLevelCore;
	public int maxLevelDatabase;
	public int maxLevelFirewall;

	private AudioSource rAudio;
	public void PlayUpgrade()
	{
		rAudio.PlayOneShot(sfxUpgrade);
	}

	private float lastPlayedSound = 0f;
	public void PlayLaser()
	{
		if (Time.time > lastPlayedSound + 0.1f) {
			rAudio.PlayOneShot(sfxLaser);
			lastPlayedSound = Time.time;
		}		
	}
	public void PlayBuild()
	{
		rAudio.PlayOneShot(sfxBuild);
	}
	public void PlayExplosion()
	{
		rAudio.PlayOneShot(sfxExplosion);
	}
	public void PlayError()
	{
		rAudio.PlayOneShot(sfxError);
	}
	public void PlayButton()
	{
		rAudio.PlayOneShot(sfxButton);
	}


	protected static MainScene s_instance;
	public static MainScene Instance
	{
		get
		{
			if (s_instance == null)
			{
				return null;
			}
			else
			{
				return s_instance;
			}
		}
	}

	protected virtual void Awake()
	{
		// Only one instance at a time!
		if (s_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		s_instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        UpdateUI(Const.UIType.None);
		rAudio = this.GetComponent<AudioSource>();
	}

    private Vector3 firstPosition;
    // Update is called once per frame
    void Update()
    {
        //Check Input
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 deltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firstPosition;
			Camera.main.transform.position += new Vector3(-1 * deltaPosition.x, -1 * deltaPosition.y, 0f);
            firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		if (Input.mouseScrollDelta.y < 0)
		{
			float size = Camera.main.orthographicSize;
			size += Time.deltaTime * 2f;
			if (size >= 4.1f) size = 4.1f;
			Camera.main.orthographicSize = size;
		}
		else if (Input.mouseScrollDelta.y > 0)
		{
			float size = Camera.main.orthographicSize;
			size -= Time.deltaTime * 2f;
			if (size <= 3f) size = 3f;
			Camera.main.orthographicSize = size;
		}

        RunPerTick();

        UpdateUI();
    }

	public void DamageShield(float dmg)
	{
		m_CurrentCash -= dmg;
		m_Cash.text = "$" + Mathf.Round(m_CurrentCash).ToString();
	}

	protected void RunPerTick()
    {
        float totalRev = 0;
        for (int i = 0; i < mChildren.Count; i++)
        {
            totalRev -= mChildren[i].GetComponent<EntityNode>().PerTick();
		}

		float diffTime = Time.deltaTime / timeMultiplier;
        m_tick += diffTime;

        m_CurrentCash += totalRev * diffTime;

        if (m_tick >= 1f)
        {
            m_CurrentDay++;
            m_tick = 0f;

			if (m_CurrentDay % 7 == 0)
			{
				CreateEnemyShip();
			}
        }
    }

    protected void UpdateUI()
    {
        m_Cash.text = "$"+Mathf.Round(m_CurrentCash).ToString();
        m_Day.text = "Day "+m_CurrentDay.ToString();

		m_HourTick.fillAmount = 1f - m_tick;
    }

    public void OpenMenuUpgrade(oServer cServer)
    {
		PlayButton();
		curServer = cServer;
        UpdateUI(Const.UIType.Upgrade);
		p_Upgrade.GetComponent<UpgradeMenu>().ShowMenu();
	}

	public void OpenMenuAdd(oServer cServer)
	{
		PlayButton();
		curServer = cServer;
		UpdateUI(Const.UIType.Add);
        p_AddNode.GetComponent<AddNewMenu>().ShowMenu();
	}

	public void OpenMenuLearn()
	{
		PlayButton();
		UpdateUI(Const.UIType.Learn);
	}

	protected void UpdateUI(Const.UIType uiType)
    {
        p_AddNode.gameObject.SetActive(false);
		p_Learn.gameObject.SetActive(false);
		p_Upgrade.gameObject.SetActive(false);
        switch (uiType)
        {
            case Const.UIType.Upgrade:p_Upgrade.gameObject.SetActive(true); break;
			case Const.UIType.Add: p_AddNode.gameObject.SetActive(true); break;
			case Const.UIType.Learn: p_Learn.gameObject.SetActive(true); break;
		}
	}

	private int difficultyLevel = 1;

	[SerializeField] GameObject prefabEnemy;
	public void CreateEnemyShip()
	{
		int totalShip = Random.Range(1, 5);

		int maxlevel = 1;
		int minlevel = 1;

		if (difficultyLevel == 1)
		{
			totalShip = 1;
		}
		else if (difficultyLevel == 2)
		{
			totalShip = 3;
		}
		else if (difficultyLevel == 3)
		{
			totalShip = 5;
		}
		else if (difficultyLevel <= 5)
		{
			minlevel = 1;
			maxlevel = 2;
			totalShip = Random.Range(5, 8);
		}
		else if (difficultyLevel <= 7)
		{
			minlevel = 1;
			maxlevel = 2;
			totalShip = Random.Range(8, 15);
		}
		else if (difficultyLevel <= 10)
		{
			minlevel = 1;
			maxlevel = 3;
			totalShip = Random.Range(12, 20);
		}
		else if (difficultyLevel <= 15)
		{
			minlevel = 2;
			maxlevel = 3;
			totalShip = Random.Range(15, 20);
		}
		else if (difficultyLevel <= 20)
		{
			minlevel = 2;
			maxlevel = 3;
			totalShip = Random.Range(18, 30);
		}
		else if (difficultyLevel <= 30)
		{
			minlevel = 2;
			maxlevel = 4;
			totalShip = Random.Range(20, 35);
		}
		else if (difficultyLevel <= 35)
		{
			minlevel = 2;
			maxlevel = 5;
			totalShip = Random.Range(25, 35);
		}
		else if (difficultyLevel <= 40)
		{
			minlevel = 3;
			minlevel = 6;
			totalShip = Random.Range(30, 40);
		}
		else if (difficultyLevel <= 45)
		{
			minlevel = 4;
			minlevel = 6;
			totalShip = Random.Range(35, 40);
		}
		else
		{
			minlevel = 3;
			minlevel = 7;
			totalShip = Random.Range(40, 50);
		}
		difficultyLevel++;

		List<oServer> activeServer = new List<oServer>();
		for (int i = 0; i < 8; i++)
		{
			oServer iServ = mChildren[i].GetComponent<oServer>();
			if (iServ != null)
			{
				if (iServ.mLevelCore > 0)
				{
					activeServer.Add(iServ);
				}
			}
		}

		for (int i = 0; i < totalShip; i++)
		{
			int level = Random.Range(minlevel, maxlevel);

			int whichServe = Random.Range(0, activeServer.Count - 1);

			oServer targetServer = activeServer[whichServe];

			GameObject nShip = Instantiate(prefabEnemy);
			EnemyShip compShip = nShip.GetComponent<EnemyShip>();

			int slevel = targetServer.mLevelCore;
			float initialAngle = Random.Range(0, 360);
			float distance = (2.2f + (level * 0.25f));
			float x = distance * Mathf.Cos(Mathf.Deg2Rad * initialAngle);
			float y = distance * Mathf.Sin(Mathf.Deg2Rad * initialAngle);
			Vector3 stayMove = new Vector3(x, y, 0) + targetServer.transform.position;
			nShip.transform.position = stayMove;

			compShip.InitShip(level, targetServer);

			mEnemy.Add(nShip);
		}
	}
    
	public void DestroyEnemyShip(GameObject nShip)
	{
		Destroy(nShip);
		mEnemy.Remove(nShip);
	}

	public void QuitGame()
	{
		PlayButton();
		SceneManager.LoadScene("TitleScene");
	}
}
