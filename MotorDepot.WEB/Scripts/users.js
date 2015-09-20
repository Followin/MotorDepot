(function() {
  $('body').on('click', '.select-wrapper', function(e) {
    return e.stopPropagation();
  });

  $('body').on('change', '.role-select', function() {
    var roleId, userId;
    roleId = $(this).val();
    userId = $(this).data('user-id');
    return $.ajax({
      type: "GET",
      url: "/Account/ChangeUserRole?userId=" + userId + "&roleId=" + roleId,
      success: function(data, statusText) {
        if (data === true) {
          return addMessage('success', 'Роль пользователя успешно изменена');
        } else {
          return addMessage('error', 'Невозможно изменить роль данному пользователю');
        }
      }
    });
  });

}).call(this);

//# sourceMappingURL=users.js.map
