namespace TinyStorage;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Intent.ActionView], Categories = [Intent.CategoryDefault, Intent.CategoryBrowsable],
    DataScheme = "tinystorage")]
public class AndroidWebAuthenticatorCallbackActivity : WebAuthenticatorCallbackActivity
{
}