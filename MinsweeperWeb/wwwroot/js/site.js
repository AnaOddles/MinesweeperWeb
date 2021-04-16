$(document).ready(function () {
    //Disable the right click \menu from popping up
    $(document).contextmenu(function () {
        return false;
    });

    //When the JS loads
    $(function () {
        console.log("page is ready");

        //Timer for countdown 

        //------------------------------------------------------------------------------------------------------------------
        // Code from Yusuf on StackOverFlow @https://stackoverflow.com/questions/5517597/plain-count-up-timer-in-javascript
        
        
        var timerVar = setInterval(countTimer, 1000);
        var totalSeconds = 0;
        var time = "";
        function countTimer() {
            console.log(totalSeconds);
            ++totalSeconds;
            var hour = Math.floor(totalSeconds / 3600);
            var minute = Math.floor((totalSeconds - hour * 3600) / 60);
            var seconds = totalSeconds - (hour * 3600 + minute * 60);
            if (hour < 10)
                hour = "0" + hour;
            if (minute < 10)
                minute = "0" + minute;
            if (seconds < 10)
                seconds = "0" + seconds;
            document.getElementById("timer").innerHTML = hour + ":" + minute + ":" + seconds;
            time = hour + ":" + minute + ":" + seconds;

            //---------------------------------------------------------------------------------------------------------------
        }

        //Mousedown event for the game board button
        $(document).on("mousedown", ".game-button", function (event) {
            event.preventDefault();
            var mineID = $(this).val();

            //right click
            if (event.button == 2) {
                checkForGameEnd(timerVar, time);
                event.preventDefault();
                console.log("right clicked on: " + mineID);
                console.log("RightClickContinueGame");
                doButtonUpdate(mineID, "/Game/RightClickContinueGame");
                checkForGameEnd(timerVar, time);
            }


            //left click
            else {
                checkForGameEnd(timerVar, time);
                event.preventDefault();
                console.log("left clicked on: " + mineID);
                console.log("ShowOneMine");
                event.stopPropagation();
                doButtonUpdate(mineID, "/Game/LeftClickContinueGame");
                checkForGameEnd(timerVar, time);

            }
        });

        //Mousedown aevent for the load game button 
        $(document).on("mousedown", ".load", function (event) {
            
            event.preventDefault();
            event.stopPropagation();
            console.log("Loading");
            getLoadedSeconds("/Game/GetLoadedSeconds");
            totalSeconds = Number($("#seconds").text());
            loadGame("/Game/OnLoad");  
            totalSeconds = Number($("#seconds").text());
        });

        //Mousedown event for the save game button
        $(document).on("mousedown", ".save", function (event) {
            console.log("saving");
            event.preventDefault();
            event.stopPropagation();
            saveGame(totalSeconds, "/Game/OnSave");

        });

    });

    //Ajax function for requesting the seconds from the loaded save
    function getLoadedSeconds(urlString) {
        $.ajax({
            method: "GET",
            url: urlString,
            datatype: "int",
            success: function (data) {
                console.log("Grabbing seconds from AJAX");
                console.log("Seconds loaded in: " + data);
                var p = document.getElementById("seconds");
                p.textContent = data;
            }
        });
    }

    //Ajax call for a click on board button
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

    //Ajax call for loading a save game
    function loadGame(urlString) {
        $.ajax({
            datatype: "json",
            method: "post",
            url: urlString,
            data:
            {
                
            },
            success: function (data) {
                console.log(data);
                
                $('#board').html(data);
            }
        });
    }

    //Ajax call saving a game
    function saveGame(totalSeconds, urlString) {
        $.ajax({
            datatype: "json",
            method: "post",
            url: urlString,
            data:
            {
                "seconds": totalSeconds
            },
            success: function (data) {
                console.log(totalSeconds);
                $('#board').html(data);
                
            }
        });
    }

    //JS Function to check if the game has ended - disables buttons
    function checkForGameEnd(timerVar, time) {
        var txt = $('#gameStatus p').text();
        var user = $('#user').text();
        console.log("user" + user);

        console.log(txt);
        if (txt.localeCompare("GameEnd") == 0)
        {
            //alert("disabled");
            document.getElementById("game-button").onclick = function () { return false; } 
            clearInterval(timerVar);
            document.getElementById("time").innerHTML = time;

        }

    } 
});



