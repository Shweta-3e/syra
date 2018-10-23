function include(e) {
    var t = document.createElement("script");
    t.src = e, t.type = "text/javascript", t.defer = !0, document.getElementsByTagName("head").item(0).appendChild(t)
}

function keyupFunction(e) {
    var t = new Date;
    if (chatText = document.getElementById("chatText").value, 13 == e.which || 13 == e.keyCode) {
        var a = '<div class="chat-message clearfix"><img src="Me.png" alt="" width="32" height="32"><div class="chat-message-content clearfix"><span class="chat-time">' + t.getHours() + ":" + t.getMinutes() + "</span><h5>Me</h5><p>" + chatText + '</p></div> \x3c!-- end chat-message-content --\x3e</div> <hr><div class="chat-message clearfix"><img src="https://whichbigdata.com/MikeBot/Assets/MH.png" alt="" width="25" height="25"><div class="chat-message-content clearfix"><span class="chat-time">' + t.getHours() + ":" + t.getMinutes() + "</span><h5>MikeHabib</h5><p>Thank You.. will get back to you shortly!!</p></div> \x3c!-- end chat-message-content --\x3e</div> ";
        $("#chatArea").append(a), document.getElementById("chatText").value = "", alert(document.getElementById("chatArea").offsetHeight), $(".chat-history").animate({
            scrollTop: 9999999
        }, 0)
    }
}

include("https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js");

var uniqueid, click_url, ConversationCounter = 0,
    FormFilled = 0,
    FirstToken = "",
    region = "",
    ip = "",
    textcol = "",
    syraimg = "";
var customerDetails = "";

function getuserdetails() {

}
//customerDetails={WelcomeMsg:"Hi, I am Partha! <br>Can I answer any Tax related questions, both Federal & State?",FirstMsg:"Hi, I am <b>Partha</b>.<br>I can help you with Tax related questions, both Federal & State.",SecondMsg:'I could help you with any of following topics. Or type in any other question that you may have.  <br><a href="javascript:void(0);" onclick="PostMsgOnClick(\'Unfiled Returns\')"><font color="black" style="text-decoration: underline;">1.Unfiled Returns or </font></a><br><a href="javascript:void(0);"onclick="PostMsgOnClick(\'Back Tax Help \')" ><font color="black" style="text-decoration: underline;">2.Back Tax Help or</font></a><br><a href="javascript:void(0);" onclick="PostMsgOnClick(\'Tax Audit Representation\')" ><font color="black" style="text-decoration: underline;">3.Tax Audit Representation or</font></a><br><a href="javascript:void(0);"onclick="PostMsgOnClick(\'941 Payroll Help\')"  ><font color="black" style="text-decoration: underline;">4.941 Payroll Help or</font></a><br><a href="javascript:void(0);" onclick="PostMsgOnClick(\'Tax Planning\')" ><font color="black" style="text-decoration: underline;">5.Tax Planning</font></a>',BaseColor:"#a4b0c1",BotSecret:"KlCSDz8jvaw.cwA.l8c.cYWWVaYd5wglO_depyLErpwc6mbPHQEAcMZoOwINqWQ",BotURI:"https://directline.botframework.com/v3",WebsiteURL:"http://mikehabibbot.azurewebsites.net",DomainName:""}

function StartExecution() {
    tracklocation(), authorize(), insertChat("ThirdEye", customerDetails.Data.FirstMsg), insertChat("ThirdEye", customerDetails.Data.SecondMsg)
}

function tracklocation() {
    var e = new XMLHttpRequest;
    e.open("GET", "http://ip-api.com/json", !0), e.send(), e.onreadystatechange = function() {
        if (4 == e.readyState && 200 == e.status) {
            var t = JSON.parse(e.responseText);
            region = t.regionName, ip = t.query, null == uniqueid && (uniqueid = uuidv4()), $.ajax({
                url: "http://mikehabibbot.azurewebsites.net/api/State/SendMail?Name=" + region + "&IPAddress=" + ip + "&UniqueId=" + uniqueid,
                type: "POST",
                ContentType: "application/json; charset=utf-8",
                data: "{}",
                crossDomain: !0
            }).done(function(e) {}).fail(function(e, t) {})
        }
    }
}

function getbase() {
    var e = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(customerDetails.Data.BaseColor),
        t = parseInt(e[1], 16),
        a = parseInt(e[2], 16),
        i = parseInt(e[3], 16);
    t /= 255, a /= 255, i /= 255;
    var o, n = Math.max(t, a, i),
        s = Math.min(t, a, i),
        r = (n + s) / 2;
    if (n == s) o = 0;
    else {
        var l = n - s;
        switch (o = r > .5 ? l / (2 - n - s) : l / (n + s), n) {
            case t:
                (a - i) / l + (a < i ? 6 : 0);
                break;
            case a:
                (i - t) / l + 2;
                break;
            case i:
                (t - a) / l + 4
        }
        6
    }
    o *= 100, o = Math.round(o), r *= 100, (r = Math.round(r)) > 50 ? (textcol = "#000000", syraimg = "https://syra.ai/genericbot/assets/syra.png") : (textcol = "#ffffff", syraimg = "https://syra.ai/genericbot/assets/syrawithbackground.png")
}

function link(e) {
    var t = e,
        a = uniqueid;
    $.ajax({
        url: "http://mikehabibbot.azurewebsites.net/api/Link/SendLink?Name=" + region + "&IPAddress=" + ip + "&UniqueId=" + a + "&Url=" + t,
        type: "POST",
        ContentType: "application/json; charset=utf-8",
        data: "{}",
        crossDomain: !0
    }).done(function(e) {}).fail(function(e, t) {})
}

function authorize() {
    var e = new XMLHttpRequest,
        t = customerDetails.Data.BotSecret;
    e.open("POST", customerDetails.Data.BotURI + "/directline/tokens/generate", !0), e.setRequestHeader("Authorization", "Bearer " + t), e.send(), e.onreadystatechange = function() {
        if (4 == e.readyState && 200 == e.status) {
            var t = JSON.parse(e.responseText);
            token = t.token;
            var a = new XMLHttpRequest;
            a.open("POST", customerDetails.Data.BotURI + "/directline/conversations", !0), a.setRequestHeader("Content-Type", "application/json"), a.setRequestHeader("Authorization", "Bearer " + token), a.send(), a.onreadystatechange = function() {
                if (4 == a.readyState && 201 == a.status) {
                    var e = JSON.parse(a.responseText);
                    convid = e.conversationId, console.log(convid), token = e.token, console.log(token), e.streamUrl
                }
            }
        }
    }
}

function AuthorizeAgain() {
    var e = new XMLHttpRequest;
    e.open("POST", customerDetails.Data.BotURI + "/directline/conversations", !0),
        e.setRequestHeader("Content-Type", "application/json"),
        e.setRequestHeader("Authorization", "Bearer " + FirstToken),
        e.send(),
        e.onreadystatechange = function () {
            if (4 == e.readyState && 201 == e.status) {
                var t = JSON.parse(e.responseText);
                convid = t.conversationId, token = t.token, t.streamUrl
            }
        };
};

window.setInterval(function() {
    AuthorizeAgain();
}, 18e5);
var com = 0;

function runScript(e) {
    if (13 == e.which || 13 == e.keyCode) {
        var t = document.getElementById("btn-input").value;
        if ("" == t) return;
        if (":" == t.charAt(0) && t.length <= 3) {
            var a = ["<i class='em em-slightly_smiling_face'></i>", "<i class='em em-drooling_face'></i>", "<i class='em em-face_with_cowboy_hat'></i>", "<i class='em em-grinning_face_with_one_large_and_one_small_eye'></i>", "<i class='em em-grinning_face_with_star_eyes'></i>", "<i class='em em-grinning'></i>", "<i class='em em-innocent'></i>", "<i class='em em-slightly_smiling_face'></i>"],
                i = Math.floor(7 * Math.random() + 1);
            return insertChat("me", a[i]), insertChat("ThirdEye", a[i = Math.floor(7 * Math.random() + 1)]), void(document.getElementById("btn-input").value = "")
        }
        var o = document.getElementById("btn-input");
        if (insertChat("me", o.value, 0), "thirdeye_demo" == o.value) {
            insertChat("ThirdEye", "Noted!!", 1e3);
            var n, s = new XMLHttpRequest;
            n = o.value, s.open("GET", "http://ip-api.com/json", !0), s.send(), s.onreadystatechange = function() {
                if (4 == s.readyState && 200 == s.status) {
                    var e = JSON.parse(s.responseText);
                    region = e.regionName, ip = e.query, null == uniqueid && (uniqueid = uuidv4()), $.ajax({
                        url: customerDetails.WebsiteURL + "/api/Demo/SendResponse?Name=" + region + "&IPAddress=" + ip + "&UniqueId=" + uniqueid + "&Response=" + n,
                        type: "POST",
                        ContentType: "application/json; charset=utf-8",
                        data: "{}",
                        crossDomain: !0
                    }).done(function(e) {}).fail(function(e, t) {})
                }
            }
        } else UserAction("me", o.value);
        return document.getElementById("btn-input").value = "", !0
    }
}

function runScriptByClick() {
    var e = document.getElementById("btn-input").value;
    if ("" !== e) {
        if (":" == e.charAt(0) && e.length <= 3) {
            var t = ["<i class='em em-slightly_smiling_face'></i>", "<i class='em em-drooling_face'></i>", "<i class='em em-face_with_cowboy_hat'></i>", "<i class='em em-grinning_face_with_one_large_and_one_small_eye'></i>", "<i class='em em-grinning_face_with_star_eyes'></i>", "<i class='em em-grinning'></i>", "<i class='em em-innocent'></i>", "<i class='em em-slightly_smiling_face'></i>"],
                a = Math.floor(7 * Math.random() + 1);
            return insertChat("me", t[a]), insertChat("ThirdEye", t[a = Math.floor(7 * Math.random() + 1)]), void(document.getElementById("btn-input").value = "")
        }
        var i = document.getElementById("btn-input");
        if (insertChat("me", i.value, 0), "thirdeye_demo" == i.value) {
            insertChat("ThirdEye", "Noted!!", 1e3);
            var o, n = new XMLHttpRequest;
            o = i.value, n.open("GET", "http://ip-api.com/json", !0), n.send(), n.onreadystatechange = function() {
                if (4 == n.readyState && 200 == n.status) {
                    var e = JSON.parse(n.responseText);
                    region = e.regionName, ip = e.query, null == uniqueid && (uniqueid = uuidv4()), $.ajax({
                        url: customerDetails.Data.BotURI + "/api/Demo/SendResponse?Name=" + region + "&IPAddress=" + ip + "&UniqueId=" + uniqueid + "&Response=" + o,
                        type: "POST",
                        ContentType: "application/json; charset=utf-8",
                        data: "{}",
                        crossDomain: !0
                    }).done(function(e) {}).fail(function(e, t) {})
                }
            }
        } else UserAction("me", i.value);
        return document.getElementById("btn-input").value = "", !0
    }
}

function windowurl() {
    return window.location.href
}

function insertChat(e, t, a) {
    var i = "",
        o = windowurl();
    new Date, "https://syra.ai/#" == o ? ("me" == e && (ConversationCounter += 1, i = '<li class="right clearfix"><span class="chat-img pull-right" style="margin-right: 10px;"><img src="https://syra.ai/genericbot/assets/Me.png" alt="User Avatar" class="img-circle" style="margin-right:15px;margin-top:33px;margin-bottom:-80px;margin-left:1000px;" height="25" width="25"/><div class="chat-body clearfix"><div class="speech-bubble-r pull-right" style="background: #ffb663;border-left-color: #ffb663"><font color="'+textcol+'">' + t + "</font></div></div></span></li>"), "ThirdEye" == e && (i = '<li class="left clearfix"><span class="chat-img pull-left"><img src="https://syra.ai/genericbot/assets/chatbot.png" alt="User Avatar" style="margin-bottom:-35px;"class="" height="25" width="25" /></span><div class="chat-body clearfix" id="speech-bubble"><div class="speech-bubble" >' + t + "</div></div></li>")) : ("me" == e && (ConversationCounter += 1, i = '<li class="right clearfix"><span class="chat-img pull-right"><img src="https://syra.ai/genericbot/assets/Me.png" alt="User Avatar" class="img-circle" style="margin-right:-10px;margin-top:33px;margin-bottom:-80px;margin-left:1000px;" height="28" width="25"/><div class="chat-body clearfix"><div class="speech-bubble-r pull-right"><font color="'+textcol+'">' + t + "</font></div></div></span></li>"), "ThirdEye" == e && (i = '<li class="left clearfix"><span class="chat-img pull-left"><img src="https://syra.ai/genericbot/assets/chatbot.png" alt="User Avatar" class="" height="25" width="25" /></span><div class="chat-body clearfix" id="speech-bubble"><div class="speech-bubble" >' + t + "</div></div></li>")), setTimeout(function() {
        $("#chatArea").append(i), $(".panel-body").stop().animate({
            scrollTop: 9999999
        }, 5000)
    }, a)
}
var j = 0;

function UserAction(e, t) {
    var a = '{"type": "message","from": {"id": "' + e + '"},"text": "' + t + '"}',
        i = "",
        o = new XMLHttpRequest;
    o.open("POST", "https://directline.botframework.com/v3/directline/conversations/"
        + convid + "/activities", !0), o.setRequestHeader("Content-Type", "application/json"),
        o.setRequestHeader("Authorization", "Bearer " + token), o.send(a),
        o.onreadystatechange = function () {
        if (4 == o.readyState && 200 == o.status) {
            JSON.parse(o.responseText);
            var e = new XMLHttpRequest;
            e.open("GET", "https://directline.botframework.com/v3/directline/conversations/"
                + convid + "/activities", !0),
                e.setRequestHeader("Authorization", "Bearer " + token), e.send(),
                e.onreadystatechange = function () {
                if (4 == e.readyState && 200 == e.status) {
                    var t = JSON.parse(e.responseText),
                        a = 1,
                        o = 0;
                    for (i = t.activities[a].text;
                        "" != i;) customerDetails.Data.Botchatname == t.activities[a].from.name &&
                            (o >= j && (i = "", i = t.activities[a].text, str = i.split(" "), 3 == str.length && "Image" == str[0] && (i = '<img src="images/' + str[1] + '" height="70px" alt="' + str[2] + '">'), insertChat("ThirdEye", i, 0), j++), o++), a++ , i = null != t.activities[a] ? t.activities[a].text : "";
                    //insertChat("ThirdEye", 'For reliable tax help or a free confidential evaluation, please <b>contact us today at [<a href="tel:1-877-788-2937"><font color="blue">1-877-788-2937</font></a>] </b>or<br><b><a href="https://www.myirstaxrelief.com/contact-us.html" target="_blank" class="clicklink"><font color="blue">Get Your Free Evaluation. </font><font color="#ffb663"><i class="glyphicon glyphicon-new-window"></i></font></a></b></a></b><script>var elements = document.getElementsByTagName("a");for(var i=0; i<elements.length; i++){if (elements[i].className == "clicklink"){elements[i].onclick = function(){var check_link=$(this).attr("href");link(check_link);}}}<\/script>')
                }
            }
        }
    }
}

function PostMsgOnClick(e) {
    document.getElementById("btn-input").value = e, event.which = 13, event.keyCode = 13, runScript(event)
}

function KycForm() {
    insertChat("ThirdEye", '<form id="sendemail" method="Post"><div class="row"><div class="col-md-12">&nbsp;<br>Please Provide The Following Information For Better Communication</div></div><div class="row"><div class="col-md-7">Full name</div></div><div class="row"><div class="col-md-7"><input type="text" class="form-control Name" id="Name" name="Name"></div></div><div class="row"><div class="col-md-7">Email id</div></div><div class="row"><div class="col-md-7"><input type="email" class="form-control Email"  id="Email" name="Email"></div></div><br><div class="row"><div class="col-md-7"><input type="button" class="form-control btn-success" id="KycButton" name="KycButton" value="SUBMIT" onclick="KycFormSubmit()"></div></div></form>', 0)
}

function uuidv4() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(e) {
        var t = 16 * Math.random() | 0;
        return ("x" == e ? t : 3 & t | 8).toString(16)
    })
}

function checkEmail() {
    for (var e = document.getElementsByClassName("Email"), t = 0; t < e.length; t++) var a = e[t].value;
    return !!/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(a) || (alert("Please provide a valid email address"), a.focus, !1)
}

function checkName() {
    for (var e = document.getElementsByClassName("Name"), t = 0; t < e.length; t++) var a = e[t].value;
    return !!a.match(/^[A-Za-z]+$/) || (alert("Username must have Characters and Alphabets"), a.focus, !1)
}

function chatbot() {
    var bot_api = "http://localhost:53903/customer/getcustomerdetails?clientid=";
    //var bot_api = "http://147.75.71.162:8585/customer/getcustomerdetails?clientid=";
    var clientid = $("script[clientID]").attr("clientID");
    console.log("Got Client Id" + clientid);
	var userreq = new XMLHttpRequest();
    userreq.open("GET", bot_api + clientid,true);
    userreq.send();
    userreq.onreadystatechange = function () {
        if (userreq.readyState === 4 && userreq.status === 200) {
            customerDetails = JSON.parse(userreq.responseText);
            if (customerDetails.Data.DomainName == null) customerDetails.Data.DomainName = "";
            getbase(), $("head").append('<head><title>TaxBot</title><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1"><link href="https://syra.ai/genericbot/assets/genericbot.css" rel="stylesheet"><link href="https://afeld.github.io/emoji-css/emoji.css" rel="stylesheet"><link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"><script src="https://ajax.googleapis.com/ajax/libs/webfont/1.6.26/webfont.js"><\/script><script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"><\/script></head>');
            var e = '<body style="overflow-x: hidden;font-family: "Tahoma", sans-serif;" id="chatbody"><script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"><\/script><style>.bubble{border-radius: 5px; box-shadow: 0 0 6px #B2B2B2; line-height: 1.3; display: inline-block; padding: 10px 18px; position: relative; vertical-align: top;text-align:left; font-family:Tahoma, sans-serif !important;}.bubble::before { background-color: ' + customerDetails.Data.BaseColor + '; content: " "; display: block; height: 16px; position: absolute; top: 11px; transform: rotate( 29deg ) skew( -35deg );-moz-transform:    rotate( 29deg ) skew( -35deg ); -ms-transform:     rotate( 29deg ) skew( -35deg ); -o-transform:      rotate( 29deg ) skew( -35deg );  -webkit-transform: rotate( 29deg ) skew( -35deg );    width:  20px;\tfont-family: Tahoma, sans-serif !important;}.speech-bubble-r {position: relative;background: ' + customerDetails.Data.BaseColor + ';border-radius: .4em;color: black;padding-right: 15px;padding-top: 12px;text-align: right;width: 300px;min-height: 45px; margin-top: 15px; margin-right: 10px; font-family: Tahoma, sans-serif !important;}.speech-bubble-r:after { content: " ";  position: absolute; right: 0; top: 50%; width: 0; height: 0; border: 20px solid transparent; border-left-color: ' + customerDetails.Data.BaseColor + ';    border-right: 0; border-bottom: 0; margin-top: -10px; margin-right: -20px; font-family: Tahoma, sans-serif !important;}</style><script type="text/javascript">function closeChat(){$("#ChatWindow").fadeOut(300);$("#chatstart").fadeToggle(); }<\/script><script>$(document).ready(function(){$("#chatstart").click(function(){$("#ChatWindow").fadeToggle();$("#chatstart").fadeToggle();  });});<\/script><a href="#" id="chatstart" class="float"><div class="chatbubble"><div class="bubble you" style="background-color: ' + customerDetails.Data.BaseColor + '"><font color="' + textcol + '">' + customerDetails.Data.WelcomeMsg + '</font></div></div><span class="badge_taxbot" style="margin-right:28px;margin-top:5px;background-color:#f72b18;">2</span><img src="https://syra.ai/genericbot/assets/chatbot.png" height="100%" width="100%" style="margin-left: 20px;"></a><div class="row pull-right float" style="width: 30%; height: 90%;position:fixed; min-width:350px;min-height:400px;font-size: 14px;" id="ChatWindow"><div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" style="line-height: 1.5" id="ChatBodyArea"><div class="row" style="background-color: ' + customerDetails.Data.BaseColor + ';border-color: #659041;margin-left: 0px;height:35px;border-top-left-radius:10px;border-top-right-radius:10px; margin-bottom: -2px;"><div class="col-md-5 col-sm-5 col-xs-5 col-lg-5"><i class="fa fa-close" style="padding-top: 8px; font-size:20px;margin-right:120px; cursor: pointer; color:' + textcol + '" onclick="closeChat()"></i></div><div class="col-md-5 col-sm-5 col-xs-5 col-lg-5"><font color="' + textcol + '" style="float: right; padding-top: 8px; margin-right:-18px;" >Custom Chatbots by</font></div><div class="col-md-2 col-sm-2 col-xs-2 col-lg-2"><a href="https://syra.ai/" target="_blank"><img src="' + syraimg + '" height="20px" width="32px" style="float: right;margin-top:8px;margin-right:5px; " /></a></div></div><div class="panel panel-primary" style="border-color: ' + customerDetails.Data.BaseColor + ';margin-bottom:1px;margin-right:-15px;border-width:3px;font-size: 14px"><div class="panel-body" style="background-color: #FFFFFF;" id="panel-body"><ul class="chat" id="chatArea"></ul></div></div><div class="buttonInside" > <input id="btn-input" type="text" class="form-control pull-right" placeholder="Ask Your Tax Related Question..." style="width:104%;margin-right:-15px;height:50px;margin-top:-10px;background-color: #FFFFFF;border-color:' + customerDetails.Data.BaseColor + '; border-width:3px; border-top-width: 2px;" onkeypress="runScript(event)"><button id="btn-click"type="button" class="btn btn-outlined btn-primary" style="background-color:' + customerDetails.Data.BaseColor + ';border:none;margin-right: -10px;color:black;font-weight:bold;right: 12px" onclick="runScriptByClick()"><font color="' + textcol + '">ASK</font></button></div></div></div></body>';
            if (customerDetails.IsSuccess) {
                $("body").append(e), window.location.hostname == customerDetails.Data.DomainName ? StartExecution() : (text = '<font color="red"><b>Sorry, my owner has not consented me to be placed in this site. I am sorry for the inconvenience.</b></font>', insertChat("ThirdEye", text, 0), $("#btn-input").attr("disabled", "true"), $("#btn-click").attr("disabled", "true"))
            }
            else {
                $("body").append(e), text = '<font color="red"><b>' + customerDetails.Message + '</b></font>', insertChat("ThirdEye", text, 0), $("#btn-input").attr("disabled", "true"), $("#btn-click").attr("disabled", "true")
            }
        }
    };
}
window.onload = chatbot;