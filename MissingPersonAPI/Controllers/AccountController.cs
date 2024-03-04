namespace Missingpreson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;

        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] UserRegisterDTO userDto)
        {
            if (ModelState.IsValid == true)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userDto.UserName;

                if (ModelState.IsValid)
                {
                    user.UserName = userDto.UserName;
                    user.Email = userDto.Email;
                    IdentityResult result = await userManager.CreateAsync(user, userDto.Password);
                    if (result.Succeeded)
                    {
                        return Ok("account Add Success");
                    }
                    else
                    {
                        return BadRequest(result.Errors.FirstOrDefault());
                    }
                }
                return BadRequest(ModelState);

            }
            return BadRequest(ModelState);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO userDto)
        {
            if (ModelState.IsValid == true)
            {
                //check - create token
                ApplicationUser user = await userManager.FindByNameAsync(userDto.UserName);
                if (user != null)//user name found
                {
                    bool found = await userManager.CheckPasswordAsync(user, userDto.Password);
                    if (found)
                    {
                        //Claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        //get role
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var itemRole in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        }
                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

                        SigningCredentials signincred =
                            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //Create token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],//url web api
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signincred
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo
                        });
                    }
                }
                return Unauthorized();

            }
            return Unauthorized();
        }
    }
}
        // ExternalLoginController.cs


        //[HttpGet("login/google")]
        //public IActionResult LoginWithGoogle()
        //{
        //    var properties = new AuthenticationProperties
        //    {
        //        RedirectUri = "/signin-google"
        //    };
        //    return Challenge(properties, "Google");
        //}

        //[HttpGet("login/facebook")]
        //public IActionResult LoginWithFacebook()
        //{
        //    var properties = new AuthenticationProperties
        //    {
        //        RedirectUri = "/signin-facebook"
        //    };
        //    return Challenge(properties, "Facebook");
        //}

        //[HttpGet("signin-google")]
        //public async Task<IActionResult> SignInWithGoogle()
        //{
        //    var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    // Get access token from authenticateResult.Properties

        //    // Use the access token to authenticate the user in your API

        //    return Ok("User authenticated with Google");
        //}

        //[HttpGet("signin-facebook")]
        //public async Task<IActionResult> SignInWithFacebook()
        //{
        //    var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    // Get access token from authenticateResult.Properties

        //    // Use the access token to authenticate the user in your API

        //    return Ok("User authenticated with Facebook");
        //}
    


