$(function() {
    var currentState;
    var smartDisplayHasChanges = function() {
        $.ajax({
            url:  "/api/Sitecore/SmartDisplay/HasChanges?currentState="+ currentState,
            type: "POST",
            dataType: "json",
            success: function (data) {
                var hasChanges = data;
                if (hasChanges) {
                    location.reload();
                }
            },
            async: true
        });
    }

    $(".smart-display").show(400,function(){
        if ($(this).attr("pageMode") === "ExperienceEditor")
            return;
        currentState = $(this).attr("currentState");
        window.setInterval(smartDisplayHasChanges, 5000);
    });
});