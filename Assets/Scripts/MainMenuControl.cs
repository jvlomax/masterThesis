using UnityEngine;
using System.Collections;
using Parse;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Security.Permissions;


public class MainMenuControl : MonoBehaviour {
    public Button RegisterButton;
    public Button FacebookButton;
    public Slider Divider;
    public InputField EmailInput;
    public InputField PasswordInput;
    public Button RedTeamButton;
    public Button BlueTeamButton;
    public Text TeamSelectText;
    public Text SelectedTeamText;
    private string selectedTeam = null;
	// Use this for initialization

    void Awake() {
        RegisterButton.onClick.AddListener(OnRegisterClick);
        FacebookButton.onClick.AddListener(OnFacebookClick);
        SelectedTeamText.text = "";
        FB.Init(null);
        
    }
    
    void Start () {
	    if(PlayerPrefs.GetInt("registered", 0) == 1){
            Application.LoadLevel(1);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnRegisterClick() {
        Hashtable ht = new Hashtable();
        ht.Add("y", -50);
        ht.Add("easetype", iTween.EaseType.easeInOutElastic);
        
        iTween.MoveTo(FacebookButton.gameObject, ht);
        iTween.MoveTo(Divider.gameObject, ht);
        ht.Add("oncomplete", "OnButtonMoveCompleteHandler");
        ht.Add("oncompletetarget", gameObject);
        ht["y"] = 50;
        iTween.MoveTo(RegisterButton.gameObject, ht);


        ht.Remove("oncomplete");
        ht.Remove("oncompletetarget");
        ht.Remove("y");
        ht.Add("x", Screen.width /2);
        ht["easetype"] = iTween.EaseType.easeInCubic;
        ht.Add("time", 0.5);
        iTween.MoveTo(EmailInput.gameObject, ht);
        iTween.MoveTo(PasswordInput.gameObject, ht);

       
        FadeInButtons();

    }

    public void OnTeamSelectButton(String button) {
        Debug.Log(button);
        if (!SelectedTeamText.gameObject.active)
        {
            SelectedTeamText.gameObject.SetActive(true);
        }

        if (button == "blue")
        {
            SelectedTeamText.text = "Blue!";
            selectedTeam = "blue";
        }
        else{
            SelectedTeamText.text = "Red!";
            selectedTeam = "red";
        }
    }

    private void FadeInButtons() {
        StartCoroutine(Fade(0.5f, 255f, 300));
    }


    private IEnumerator Fade(float start, float end, float length) {
        for (float i = 0.0f; i < 1.0; i += Time.deltaTime*(1/length))
        {
            RedTeamButton.image.color =  new Color(RedTeamButton.image.color.r, RedTeamButton.image.color.g, RedTeamButton.image.color.b,Mathf.Lerp(start, end, i));
            BlueTeamButton.image.color = new Color(BlueTeamButton.image.color.r, BlueTeamButton.image.color.g, BlueTeamButton.image.color.b, Mathf.Lerp(start, end, i));
            TeamSelectText.color = new Color(TeamSelectText.color.r, TeamSelectText.color.g, TeamSelectText.color.b, Mathf.Lerp(start, end, i));
            yield return new WaitForFixedUpdate();
            



        }
    }

    public void OnButtonMoveCompleteHandler() {
        Debug.Log("onComplete");
        Text registerButtonText = RegisterButton.GetComponentInChildren<Text>();
        registerButtonText.text = "Done";
        if (Divider != null)
        {
            //Destroy(Divider.gameObject);    
        }

        if (FacebookButton != null)
        {
           // Destroy(FacebookButton.gameObject);
        }
        Debug.Log(RegisterButton.onClick);
        
        RegisterButton.onClick.RemoveListener(OnRegisterClick);
        RegisterButton.onClick.AddListener(OnDoneClick); 
        
    }

    public void OnDoneClick() {
        Debug.Log("onDoneClick");

        String email = EmailInput.text;
        String password = PasswordInput.text;
        Debug.Log("Email: " + email + " Pass: " + password);
        if (selectedTeam == null)
        {
            return;
        }
        //TODO: verify input
        PlayerPrefs.SetInt("registered", 1);
        AutoFade.LoadLevel(1, 1f, 1.5f, Color.black);
        //Application.LoadLevel(1);
        //Register(email, password, selectedTeam);
        
    }

    public void OnFacebookClick() {
        FB.Login("public_profile,email", FacebookLoginCallback);
    }

    public void FacebookLogin() {
        //Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync()
        Debug.Log("login");
       
    }

    private void FacebookLoginCallback(FBResult result) {
        Debug.Log(result.Text);
    }

    public void Register(string email, string password, string team) {
        var user = new ParseUser()
        {
            Password = password,
            Username = email
            
        };
        user["Team"] = team;
        try{
            user.SignUpAsync().ContinueWith(t => {
                if (t.IsFaulted) {
                    using (System.Collections.Generic.IEnumerator<System.Exception> enumerator = t.Exception.InnerExceptions.GetEnumerator()) {
                        if(enumerator.MoveNext()){
                            ParseException error = (ParseException) enumerator.Current;
                            Debug.LogError("Parse exception: " + error.Message);
//TODO: handle parse exceptions
                        }
                    }
                }
            });
        }catch(InvalidOperationException e){
            Debug.LogError("error in code: " + e.Message);
        }
    }
}
