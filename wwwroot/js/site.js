// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



// ***********************************************************************************************
// EDIT DELETE MODAL


//open and fill modal edit dialog

//eg: loadModalWithContent('editModal', '/Datatable/EditCustomer', 'SDA33DW1AFS')
function loadModalWithContent(modalDialogId, modalAction, strId) {
    openModalWithContent(modalDialogId, modalAction, {
        'Id': strId
    });
}
//eg: updateModalWithContent('editModal', '/Datatable/SaveCustomer', {Campo1='xxxxx',Campo2='xxxx', ecc...})
function updateModalWithContent(modalDialogId, modalAction, jsonParams) {
    openModalWithContent(modalDialogId, modalAction, jsonParams);
}
function openModalWithContent(modalDialogId, modalAction, jsonParams) {
    fetch(modalAction, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(jsonParams)
    })
        .then(response => response.text())
        .then(html => {
            document.getElementById(modalDialogId).innerHTML = html;  //inserisco contenuto in dialog modale

            //AZIONI DA FARE AL CLICK BOTTONE
            var isModalACTION_CLOSE = $('#' + modalDialogId).find('[name="IsModalACTION"]').val() == 'CLOSE';
            if (isModalACTION_CLOSE) { $('#' + modalDialogId).modal('hide'); } //nascondi modal
            else { $('#' + modalDialogId).modal('show'); } //mostra modal
            var isPageACTION_RELOAD = $('#' + modalDialogId).find('[name="IsPageACTION"]').val() == 'RELOAD';
            if (isPageACTION_RELOAD) { location.reload(true); } //ricarica pagina dal server (ie: no cache)
            var isPageREDIRECT = $('#' + modalDialogId).find('[name="IsPageREDIRECT"]').val();
            if (isPageREDIRECT != "") { location.href = isPageREDIRECT; } //ridireziona su altra pagina
        })
        .catch(error => console.error('Errore:', error));
}

// ***********************************************************************************************
// AUTOCOMPLETE CLIENT E SERVER ...chiamate da AutocompleteTagHelper (ErpComponentTagHelper)


//$(document).ready(function () {
//    $('.autocomplete-input').each(function () {
//        var input = $(this);
//        var resultsDivId = input.data('name') + 'AutocompleteResults';
//        var selectedItemsDivId = input.data('selected-items-div-id');
//        var resultsDiv = $('#' + resultsDivId);
//        var selectedItemsDiv = $('#' + selectedItemsDivId);
//        var maxSelections = input.data('max-selections');
//        var minChars = input.data('min-chars'); // Ottieni il numero di caratteri minimo
//        var mode = input.data('mode'); // Ottieni la modalità di autocomplete
//        var allChoices = [];

//        resultsDiv.hide();

//        if (mode === 'autocompleteClient') {
//            // Modalità client: Precarica tutte le scelte
//            var controller = input.data('controller');
//            var action = input.data('action');

//            $.get('/' + controller + '/' + action, function (data) {
//                if (data.error) {
//                    var validationMessage = $('span[data-valmsg-for=\"' + input.data('name') +'\"]');
//                    validationMessage.text(data.error);
//                    validationMessage.show();
//                }
//                else {
//                    allChoices = data;
//                    console.log('All choices loaded:', allChoices);
//                }

//                // Carica i valori pre-selezionati
//                var preSelectedList = input.data('pre-selected');
//                var preSelected = preSelectedList.preSelected;
//                //var preSelected = JSON.parse(preSelectedJson);
//                if (preSelected) {
//                    preSelected.forEach(function (value) {
//                        var item = allChoices.find(c => c.value === value);
//                        if (item) {
//                            if (!isItemAlreadySelected(value, selectedItemsDiv)) {
//                                addSelectedItem(item.label, item.value, input, selectedItemsDiv);
//                            }
//                        }
//                    });
//                    toggleInputVisibility(input, selectedItemsDiv, maxSelections);
//                }
//            });

//            input.on('input', function () {
//                var term = $(this).val().toLowerCase();
//                resultsDiv.empty();
//                if (term.length >= minChars) {
//                    var filtered = allChoices.filter(c => c.label.toLowerCase().includes(term));
//                    if (filtered.length) {
//                        resultsDiv.show();
//                        filtered.forEach(function (item) {
//                            resultsDiv.append('<div class="autocomplete-item" data-value="' + item.value + '" data-label="' + item.label + '">' + item.label + '</div>');
//                        });
//                        adjustResultsDivWidth(input, resultsDiv);
//                    } else {
//                        resultsDiv.hide();
//                    }
//                } else {
//                    resultsDiv.hide();
//                }
//            });

//        } else if (mode === 'autocompleteServer') {
//            // Modalità server: Richiede i dati ad ogni variazione del termine di ricerca
//            var controller = input.data('controller');
//            var action = input.data('action');
//            var preloadAction = input.data('preload-action');

//            // Carica i valori pre-selezionati
//            var preSelectedList = input.data('pre-selected');
//            var preSelected = preSelectedList.preSelected;
//            if (preSelected) {
//                $.post('/' + controller + '/' + preloadAction, { values: preSelected }, function (data) {
//                    if (data.error) {
//                        var validationMessage = $('span[data-valmsg-for=\"' + input.data('name') + '\"]');
//                        validationMessage.text(data.error);
//                        validationMessage.show();
//                    }
//                    else {
//                        data.forEach(function (item) {
//                            if (!isItemAlreadySelected(value, selectedItemsDiv)) {
//                                addSelectedItem(item.label, item.value, input, selectedItemsDiv);
//                            }
//                        });
//                        toggleInputVisibility(input, selectedItemsDiv, maxSelections);
//                    }
//                });
//            }

//            input.on('input', function () {
//                var term = $(this).val().toLowerCase();
//                resultsDiv.empty();
//                if (term.length >= minChars) {
//                    $.get('/' + controller + '/' + action, { term: term }, function (data) {
//                        if (data.error) {
//                            var validationMessage = $('span[data-valmsg-for=\"' + input.data('name') + '\"]');
//                            validationMessage.text(data.error);
//                            validationMessage.show();
//                        }
//                        else if (data.length) {
//                            resultsDiv.show();
//                            data.forEach(function (item) {
//                                resultsDiv.append('<div class="autocomplete-item" data-value="' + item.value + '" data-label="' + item.label + '">' + item.label + '</div>');
//                            });
//                            adjustResultsDivWidth(input, resultsDiv);
//                        } else {
//                            resultsDiv.hide();
//                        }
//                    });
//                } else {
//                    resultsDiv.hide();
//                }
//            });
//        }

//        function adjustResultsDivWidth(input, resultsDiv) {
//            resultsDiv.css('width', input.outerWidth() + 'px');
//        }

//        input.on('focus', function () {
//            adjustResultsDivWidth(input, resultsDiv);
//        });

//        var isSelectingItem = false;

//        $(document).on('mousedown', '.autocomplete-item', function () {
//            isSelectingItem = true;
//        });

//        $(document).on('mouseup', '.autocomplete-item', function () {
//            isSelectingItem = false;
//        });

//        input.on('blur', function () {
//            setTimeout(function () {
//                if (!isSelectingItem) {
//                    resultsDiv.hide();
//                }
//            }, 100);
//        });

//        $(document).on('click', '#' + resultsDivId + ' .autocomplete-item', function () {
//            var label = $(this).data('label');
//            var value = $(this).data('value');
//            if (!isItemAlreadySelected(value, selectedItemsDiv)) {
//                addSelectedItem(label, value, input, selectedItemsDiv);
//            }
//            input.val('');
//            resultsDiv.hide();
//        });

//        $(document).on('click', '#' + selectedItemsDivId + ' .remove-item', function () {
//            $(this).parent().remove();
//            toggleInputVisibility(input, selectedItemsDiv, maxSelections);
//        });

//        function isItemAlreadySelected(value, selectedItemsDiv) {
//            return selectedItemsDiv.find('.selected-item[data-value="' + value + '"]').length > 0;
//        }

//        function addSelectedItem(label, value, input, selectedItemsDiv) {
//            var itemDiv = $('<div class="selected-item" data-value="' + value + '">' + label + ' <span class="remove-item">&times;</span></div>');
//            var inputField = $('<input type="hidden" name="' + input.data('name') + '" value="' + value + '" />');
//            itemDiv.append(inputField);
//            selectedItemsDiv.append(itemDiv);
//            toggleInputVisibility(input, selectedItemsDiv, maxSelections);
//        }

//        function toggleInputVisibility(input, selectedItemsDiv, maxSelections) {
//            var selectedCount = selectedItemsDiv.children().length;
//            if (maxSelections > 0 && selectedCount >= maxSelections) {
//                input.hide();
//            } else {
//                input.show();
//            }
//        }

//        // Initial toggle in case there are pre-selected items
//        toggleInputVisibility(input, selectedItemsDiv, maxSelections);
//    });
//});



$(document).ready(function () {
    $('.autocomplete-input').each(function () {
        var input = $(this);
        var resultsDivId = input.data('name') + 'AutocompleteResults';
        var selectedItemsDivId = input.data('selected-items-div-id');
        var resultsDiv = $('#' + resultsDivId);
        var selectedItemsDiv = $('#' + selectedItemsDivId);
        var maxSelections = input.data('max-selections');
        var minChars = input.data('min-chars');
        var mode = input.data('mode');
        var allChoices = [];
        var cache = {};

        resultsDiv.hide();

        function loadChoices(callback) {
            if (mode === 'autocompleteClient') {
                var controller = input.data('controller');
                var action = input.data('action');

                $.get('/' + controller + '/' + action, function (data) {
                    if (data.error) {
                        showValidationMessage(input.data('name'), data.error);
                    } else {
                        allChoices = data;
                        console.log('All choices loaded:', allChoices);
                        callback();
                    }
                });
            } else {
                callback();
            }
        }

        loadChoices(function () {
            var preSelectedList = input.data('pre-selected');
            var preSelected = preSelectedList.preSelected;
            if (preSelected) {
                preSelected.forEach(function (value) {
                    var item = allChoices.find(c => c.value === value);
                    if (item && !isItemAlreadySelected(value, selectedItemsDiv)) {
                        addSelectedItem(item.label, item.value, input, selectedItemsDiv);
                    }
                });
                toggleInputVisibility(input, selectedItemsDiv, maxSelections);
            }
        });

        input.on('input', function () {
            var term = $(this).val().toUpperCase();
            resultsDiv.empty();
            if (term.length >= minChars) {
                if (mode === 'autocompleteClient') {
                    var filtered = allChoices.filter(c => (' ' + c.label.toUpperCase() + ' ').includes(term));
                    showResults(filtered);
                } else if (mode === 'autocompleteServer') {
                    if (cache[term]) {
                        showResults(cache[term]);
                    } else {
                        var controller = input.data('controller');
                        var action = input.data('action');
                        $.get('/' + controller + '/' + action, { term: term }, function (data) {
                            if (data.error) {
                                showValidationMessage(input.data('name'), data.error);
                            } else {
                                cache[term] = data;
                                showResults(data);
                            }
                        });
                    }
                }
            } else {
                resultsDiv.hide();
            }
        });

        // Gestione del click sull'icona
        input.next('.autocomplete-icon').on('click', function () {
            if (resultsDiv.is(':visible')) {
                resultsDiv.hide();
            } else {
                if (mode === 'autocompleteClient') {
                    showResults(allChoices);
                } else if (mode === 'autocompleteServer') {
                    var controller = input.data('controller');
                    var action = input.data('action'); var term = '%';
                    $.get('/' + controller + '/' + action, { term: term }, function (data) {
                        if (data.error) {
                            showValidationMessage(input.data('name'), data.error);
                        } else {
                            showResults(data);
                        }
                    });
                }
            }
        });

        function showResults(items) {
            if (items.length) {
                resultsDiv.empty();
                resultsDiv.show();
                items.forEach(function (item, index) {
                    resultsDiv.append('<div class="autocomplete-item" tabindex="0" role="option" data-value="' + item.value + '" data-label="' + item.label + '">' + item.label + '</div>');
                });
                adjustResultsDivWidth(input, resultsDiv);
            } else {
                resultsDiv.hide();
            }
        }


        function adjustResultsDivWidth(input, resultsDiv) {
            resultsDiv.css('width', input.outerWidth() + 'px');
        }

        input.on('focus', function () {
            adjustResultsDivWidth(input, resultsDiv);
        });

        var isSelectingItem = false;

        $(document).on('mousedown', '.autocomplete-item', function () {
            isSelectingItem = true;
        });

        $(document).on('mouseup', '.autocomplete-item', function () {
            isSelectingItem = false;
        });

        input.on('blur', function () {
            setTimeout(function () {
                if (!isSelectingItem) {
                    resultsDiv.hide();
                }
            }, 100);
        });

        $(document).on('click', '#' + resultsDivId + ' .autocomplete-item', function () {
            var label = $(this).data('label');
            var value = $(this).data('value');
            if (!isItemAlreadySelected(value, selectedItemsDiv)) {
                addSelectedItem(label, value, input, selectedItemsDiv);
            }
            input.val('');
            resultsDiv.hide();
        });

        $(document).on('click', '#' + selectedItemsDivId + ' .remove-item', function () {
            $(this).parent().remove();
            toggleInputVisibility(input, selectedItemsDiv, maxSelections);
        });

        function isItemAlreadySelected(value, selectedItemsDiv) {
            return selectedItemsDiv.find('.selected-item[data-value="' + value + '"]').length > 0;
        }

        function addSelectedItem(label, value, input, selectedItemsDiv) {
            var itemDiv = $('<div class="selected-item" data-value="' + value + '">' + label + ' <span class="remove-item">&times;</span></div>');
            var inputField = $('<input type="hidden" name="' + input.data('name') + '" value="' + value + '" />');
            itemDiv.append(inputField);
            selectedItemsDiv.append(itemDiv);
            toggleInputVisibility(input, selectedItemsDiv, maxSelections);
        }

        function toggleInputVisibility(input, selectedItemsDiv, maxSelections) {
            var selectedCount = selectedItemsDiv.children().length;
            if (maxSelections > 0 && selectedCount >= maxSelections) {
                input.hide();
            } else {
                input.show();
            }
        }

        // Initial toggle in case there are pre-selected items
        toggleInputVisibility(input, selectedItemsDiv, maxSelections);
    });
});



