// public  enum Roles
//     {    
//         User,
//         Admin,
//     } 
public class Users{
    public Guid Id {get;set;}
    public required string Name{get;set;}

    public required string Email {get;set;}

     public required string Password {get;set;}

    
    // public Roles Roles { get; set; } = Roles.User;

    public string Roles{get;set;}
}