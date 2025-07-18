/*=========================================================================================
	File Name: sweet-alerts.js
	Description: A beautiful replacement for javascript alerts
	----------------------------------------------------------------------------------------
	Item Name: Apex - Responsive Admin Theme
	Version: 1.0
	Author: GeeksLabs
	Author URL: http://www.themeforest.net/user/geekslabs
==========================================================================================*/
$(document).ready(function () {

  $('#basic-alert').on('click', function () {
    swal("Here's a message!");
  });

  $('#with-title').on('click', function () {
    swal("Here's a message!", "It's pretty, isn't it?");
  });

  $('#html-alert').on('click', function () {
    swal({
      title: 'HTML <small>Title</small>!',
      text: 'A custom <span style="color:#F6BB42">html<span> message.',
      html: true
    });
  });

  $('#type-question').on('click', function () {
    swal("Question", "Are You Sure?", "question");
  });

  $('#type-success').on('click', function () {
    swal("Good job!", "You clicked the button!", "success");
  });

  $('#type-info').on('click', function () {
    swal("Info!", "You clicked the button!", "info");
  });

  $('#type-warning').on('click', function () {
    swal("Warning!", "You clicked the button!", "warning");
  });

  $('#type-error').on('click', function () {
    swal("Error!", "You clicked the button!", "error");
  });

  $('#custom-icon').on('click', function () {
    swal({
      title: "Sweet!",
      text: "Here's a custom image.",
      imageUrl: "app-assets/img/icons/thumbs-up.jpg"
    });
  });

  $('#auto-close').on('click', function () {
    swal({
      title: "Auto close alert!",
      text: "I will close in 2 seconds.",
      timer: 2000,
      showConfirmButton: false
    });
  });

  $('#outside-click').on('click', function () {
    swal({
      title: 'Click outside to close!',
      text: 'This is a cool message!',
      allowOutsideClick: true
    });
  });


  // Ajax Request
  $('#ajax-request').on('click', function () {
    swal({
      title: 'Submit email to run ajax request',
      input: 'email',
      showCancelButton: true,
      confirmButtonText: 'Submit',
      showLoaderOnConfirm: true,
      preConfirm: function () {
        return new Promise(function (resolve) {
          setTimeout(function () {
            resolve();
          }, 2000);
        });
      },
      allowOutsideClick: false
    }).then(function (email) {
      if (email) {
        swal({
          type: 'success',
          title: 'Ajax request finished!',
          html: 'Submitted email: ' + email
        });
      }
    })
  });

  // Button Options
  $('#custom-button').on('click', function () {
    swal({
      title: '<i>HTML</i> <u>example</u>',
      type: 'info',
      html: 'You can use <b>bold text</b>, ' +
        '<a href="//github.com">links</a> ' +
        'and other HTML tags',
      showCloseButton: true,
      showCancelButton: true,
      confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
      cancelButtonText: '<i class="fa fa-thumbs-down"></i> Opps!'
    })
  });

  // Prompt Function
  $('#prompt-function').on('click', function () {
    //swal({
    //  title: "An input!",
    //  text: "Write something interesting:",
    //  input: "text",
    //  showCancelButton: true,
    //  closeOnConfirm: false,
    //  animation: "slide-from-top",
    //  inputPlaceholder: "Write something"
    //}).then(function (inputValue) {
    //  if (inputValue === false) return false;
    //  if (inputValue === "") {
    //    swal.showInputError("You need to write something!");
    //    return false
    //  }
    //  swal("Nice!", "You wrote: " + inputValue, "success");
    //});
      swal({
          title: 'Are you sure?',
          text: "You won't be able to revert this!",
          type: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#0CC27E',
          cancelButtonColor: '#FF586B',
          confirmButtonText: 'Text Changed',
          cancelButtonText: "No, cancel"
      }).then(function (isConfirm) {
          if (isConfirm) {
              swal(
                  'Changed!',
                  'Confirm button text was changed!!',
                  'success'
              );
          }
      }).catch(swal.noop);
  });

  // Confirm Button Action
    
  $('#confirm-text').on('click', function () {
    swal({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#0CC27E',
      cancelButtonColor: '#FF586B',
      confirmButtonText: 'Text Changed',
      cancelButtonText: "No, cancel"
    }).then(function (isConfirm) {
      if (isConfirm) {
        swal(
          'Changed!',
          'Confirm button text was changed!!',
          'success'
        );
      }
    }).catch(swal.noop);
  });

  // Confirm & Cancel Button
  $('#confirm-cancel').on('click', function () {
    swal({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#0CC27E',
      cancelButtonColor: '#FF586B',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel!',
      confirmButtonClass: 'btn btn-success btn-raised mr-5',
      cancelButtonClass: 'btn btn-danger btn-raised',
      buttonsStyling: false
    }).then(function () {
      swal(
        'Deleted!',
        'Your imaginary file has been deleted.',
        'success'
      )
    }, function (dismiss) {
      // dismiss can be 'overlay', 'cancel', 'close', 'esc', 'timer'
      if (dismiss === 'cancel') {
        swal(
          'Cancelled',
          'Your imaginary file is safe :)',
          'error'
        )
      }
    })
  });

  // Chaining modals / Steps
  $('#steps').on('click', function () {
    swal.setDefaults({
      confirmButtonText: 'Next &rarr;',
      showCancelButton: true,
      cancelButtonColor: '#FF586B',
      animation: false
    });

    var steps = [{
        title: 'Step 1',
        text: 'Chaining modals is easy with Step 1'
      },
      {
        title: 'Step 2',
        text: 'Chaining modals is easy with Step 2'
      },
      {
        title: 'Step 3',
        text: 'Chaining modals is easy with Step 3'
      },
    ];

    swal.queue(steps).then(function () {
      swal({
        title: 'All done!',
        text: 'Great job :)',
        confirmButtonText: 'Lovely!',
        showCancelButton: false
      });
    }).then(function () {
      swal.resetDefaults();
    }).catch(swal.noop);
  });














});
