$('body').on('click', '.select-wrapper', (e) ->
    e.stopPropagation()
)


$('body').on('change', '.role-select', ->
    roleId = $(this).val()
    userId = $(this).data('user-id')
    
    $.ajax(
        type: "GET"
        url: "/Account/ChangeUserRole?userId=#{userId}&roleId=#{roleId}"
        success: (data,statusText) ->
            if data == true
                addMessage('success', 'Роль пользователя успешно изменена')
            else addMessage('error', 'Невозможно изменить роль данному пользователю')
     )
                
)
