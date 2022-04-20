public abstract class BaseController {
    protected RequestCode requestCode = RequestCode.None;
    public RequestCode RequestCode{
        get {  return requestCode; }
    }

}
