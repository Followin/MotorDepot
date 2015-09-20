# CoffeeScript

jQuery.fn.insertFormValidation = ->
    $form = $(this)
    $form.validateParse()
    changeFormClass = (inputEl) ->
    
        el = $(inputEl)
        formGroup = el.closest('.form-group')
        if el.is(':focus')  then formGroup.addClass('has-focus') else formGroup.removeClass('has-focus')
        if el.val() == '' then formGroup.addClass('is-empty') else formGroup.removeClass('is-empty')
        
    


    $form.find('.form-group:has("textarea")').addClass('text-area-group')
    $form.find('textarea').autogrow()         

    $form.find('input[type="radio"]').closest('.form-group').addClass('radio-group')
    $form.find('select').closest('.form-group').addClass('select-group')
    $form.find("input[type='checkbox']").closest('.form-group').addClass('checkbox-group')
    $form.find("input[type='file']").closest('.form-group').addClass('file-group')


    inputValGroups = $form.find('.form-group:has("input"), .form-group:has("textarea")')


    for group in inputValGroups 
        input = $(group).find("input[type='text'], input[type='password'],
         input[type='email'], input[type='number'], input[type='date'],
          input[type='time'],textarea")
        input.on('focus', -> changeFormClass(this))
        input.on('blur', -> changeFormClass(this))
        changeFormClass(input)
        


$('form').insertFormValidation()

objHeight = 0
$.each($('form.ms-form').children(":not(:hidden)"), ->
    objHeight += $(this).height()
)
viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0
$('body').height(Math.max(viewportHeight, objHeight))




$(".action-button").clickRippleEffect()

animating = false
$(".next").click( (e) ->
    return false if animating
    
    current_fs = $(this).closest('fieldset')
    inputs = current_fs.find("input:not([type='submit']), textarea")
    
    valid = true
    for input in inputs
        valid = false if $(input).valid() is 0
    return if valid == false
    
    animating = true
    next_fs = $(this).closest('fieldset').next()
    
    next_fs.show()
    
    current_fs.animate( 
        {opacity: 0},
        step: (now, mx) ->
            scale = 1 - (1 - now) * 0.2
            left = (now * 50) + "%"
            opacity = 1 - now
            
            current_fs.css(
                'transform': 'scale('+scale+')'
            )
            next_fs.css(
                'left': left
                'opacity': opacity
            )
        duration: 800,
        complete: ->
            current_fs.hide()
            animating = false
            $('body').animate({
                scrollTop: 0
            }, 400)
        easing: 'easeInOutBack'
        )
    
)

$('.to-cars').click( (e) ->

    current_fs = $(this).closest('fieldset')
    inputs = current_fs.find("input:not([type='submit']), textarea")
    
    valid = true
    for input in inputs
        valid = false if $(input).valid() is 0
    return if valid == false
    
    startTimeString = $('#RequestedStartDate').val() + ' ' + $('#RequestedStartTime').val()
    endTimeString = $('#RequestedEndDate').val() + ' ' + $('#RequestedEndTime').val()
    $.ajax(
        type: "GET"
        url: "/Vehicle/FreeVehicles?startTime=#{startTimeString}&endTime=#{endTimeString}"
        success: (data, statusText) ->
            if data != ""
                $('.auto-chooser').empty()
                $('.auto-chooser').append(data)
    )
)

$('.previous').click( (e) -> 
    return false if animating
    animating = true
    
    current_fs = $(this).closest('fieldset')
    previous_fs = $(this).closest('fieldset').prev()
    
    previous_fs.show()
    current_fs.animate(
        {opacity: 0},
        step: (now, mx) ->
            scale = 0.8 + (1-now) * 0.2
            left = ((1-now)*50) + "%"
            opacity = 1 - now
            current_fs.css('left': left)
            previous_fs.css(
                'transform': "scale(#{scale})"
                'opacity': opacity
            )
        duration: 800,
        complete: ->
            current_fs.hide()
            animating = false
        easing: 'easeInOutBack'
    )
)