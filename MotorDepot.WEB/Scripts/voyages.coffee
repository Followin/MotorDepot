$('body').on('click', '.voyage-request', (e) ->
    $link = $(e.target)
    return if $link.is('.success')
    $link.addClass('waiting')
    
    id = $link.data('voyage-id')
    $.ajax(
        type: "GET"
        url: "/Voyage/RequestVoyage?voyageId=#{id}"
        success: (data, statusText) ->
            if data != ""
                if data == true
                    $link.removeClass('waiting error').addClass('success')
                    $link.html('Заявка подана')
                else
                    $link.removeClass('waiting').addClass('error')
                    $link.html('На данный момент, вы не можете подать заявку на данный рейс')
         )
)

$('body').on('click', '.to-requests-button', (e) ->
    $paperBlock = $(e.target).closest('.paper-block')
    id = $paperBlock.find("[name='voyageId']").val()
    $flipContainer = $("<div class='flip-container'></div>")
    $newblock = $("<div class='paper-block flip-side'></div>")
    $newblock.height($paperBlock.height())
    
    $.ajax(
        type: "GET"
        url: "/Voyage/RequestsForVoyage?voyageId=#{id}"
        success: (data, statusText) ->
            if data != ""
                $newblock.append(data)
    )
    
    
    
    $flipContainer.insertBefore($paperBlock)
    $flipContainer.append($paperBlock).append($newblock)
    $flipContainer.addClass('flipped')
)


backToVoyage = (e) ->
    $container = $(e.target).closest('.flip-side')
    $flipContainer = $container.parent()
    $flipContainer.removeClass('flipped')
    
    setTimeout( -> 
        $mainBlock = $flipContainer.find('.voyage-info')
        $mainBlock.insertBefore($flipContainer)
        $flipContainer.remove()
    , 1600)
    

$('body').on('click', '.accept-request-button', (e) ->
    $container = $(e.target).closest('.paper-block')
    $flipContainer = $container.parent()
    $mainContainer = $flipContainer.find('.voyage-info')
    
    voyageId = $flipContainer.find(".voyage-info input[name='voyageId']").val()
    
    $radios = $container.find(".form-group input[type='radio']")
    $checked = $radios.filter(':checked')
    if $checked.length > 0
        $.ajax(
            type: "GET"
            url: "/Voyage/AcceptRequest?voyageId=#{voyageId}&driverId=#{$checked.val()}"
            success: (data, statusText) ->
                if data == true
                    $.ajax(
                        type: "GET"
                        url: "/Voyage/Details?voyageId=#{voyageId}"
                        success: (data, statusText) ->
                            $mainContainer.empty().append(data)
                            backToVoyage(e)
                            
                    ) 
        )
)

$('body').on('click', '.back-to-voyage', backToVoyage)

$('body').on('change', '.order-select, .status-filter', ->
    orderStr = $('.order-select').val()
    statusStr = $('.status-filter').val()
    actionName = $(this).closest('.filters-block').data('action')
    $.ajax(
        type: "GET"
        url: "/Voyage/#{actionName}?sortOrder=#{orderStr}&status=#{statusStr}"
        success: (data, statusText) ->
            $('.voyages-list').empty().append(data)
     )
)
        