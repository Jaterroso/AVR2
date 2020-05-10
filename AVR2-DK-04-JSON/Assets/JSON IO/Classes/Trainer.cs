using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Trainer
{
    // Properties
    public string image;
    public string firstname;
    public string lastname;
    public string company;
    public string email;
    public string phone;

    // Methods
    public void ChangeEmail(string _newEmail)
    {
        email = _newEmail;
    }

    public string Print()
    {
        return firstname + " " + lastname + " is working for " + company + " and his/her contact info is " + email + "/" + phone + ".";
    }
}
