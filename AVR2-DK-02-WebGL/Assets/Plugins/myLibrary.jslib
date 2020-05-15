var myLibrary = {
    Hello: function () {
      window.alert("Hello WebGL!");
    }
};

mergeInto(LibraryManager.library, myLibrary);