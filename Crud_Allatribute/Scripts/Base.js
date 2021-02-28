var globalGridSetting = (function () {
    let globalResetState = false;
    let sortOrder = "asc";
    return {
        setResetState: function (_state) {
            globalResetState = _state;
        },
        getResetState: function () {
            return globalResetState;
        },
        setSortOrder: function (_sortOrder) {
            sortOrder = _sortOrder;
        },
        getSortOrder: function () {
            return sortOrder;
        }
    };
})();