$(document).ready(function () {
    //Disable the right click event on other areas other than buttons
    $(document).contextmenu(function () {
        return false;
    });

    $(function () {
        console.log("page is ready");

        //$(".game-button").mousedown(function (event) {
        //    event.preventDefault();
        //    var mineID = $(this).val();

        //    //if right click
        //    if (event.which == 3) {
        //        console.log("right clicked on: " + mineID);
        //        doButtonUpdate(mineID, "/Game/ShowOneMineRightClick");
        //    }
        //    else {
        //        console.log("left clicked on: " + mineID);
        //        doButtonUpdate(mineID, "/Game/ShowOneMine");
        //    }
        //});

        $(document).on("mousedown", ".game-button", function (event) {
            event.preventDefault();
            var mineID = $(this).val();
            //var gameStatus = $("#game-status").val();

            //console.log(gameStatus);
            //right click
            if (event.button == 2) {
                checkForGameEnd();
                event.preventDefault();
                console.log("right clicked on: " + mineID);
                console.log("ShowOneMine");
                doButtonUpdate(mineID, "/Game/ShowOneMineRightClick");
                checkForGameEnd();
            }

            else {
                checkForGameEnd();
                event.preventDefault();
                console.log("left clicked on: " + mineID);
                console.log("ShowOneMine");
                doButtonUpdate(mineID, "/Game/ShowOneMine");
                checkForGameEnd();

            }
        });

        $(document).on("mousedown", ".load", function (event) {

            
                loadGame( "/Game/OnLoad");
                
        });

    });

    function doButtonUpdate(mineID, urlString) {
        $.ajax({
            datatype: "json",
            method: "post",
            url: urlString,
            data:
            {
                "mineID" : mineID
            },
            success: function (data)
            {
                console.log(data);
                $('#board').html(data);
            }
        });  
    }

    function loadGame(urlString) {
        $.ajax({
            datatype: "json",
            method: "post",
            url: urlString,
            data:
            {
                "mineID": mineID
            },
            success: function (data) {
                console.log(data);
                $('#board').html(data);
            }
        });
    }

    function checkForGameEnd() {
        var txt = $('#gameStatus p').text();
        var user = $('#user').text();
        console.log("user" + user);

        console.log(txt);
        if (txt.localeCompare("GameEnd") == 0)
        {
            //alert("disabled");
            document.getElementById("game-button").onclick = function () { return false; } 
        }

    }



   
});


