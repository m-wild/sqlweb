import 'ace-builds/src-noconflict/ace';
import 'ace-builds/src-noconflict/mode-sqlserver';
import 'ace-builds/src-noconflict/theme-tomorrow';
import 'ace-builds/src-noconflict/theme-github';
// todo: have a look at https://codemirror.net/ instead of ace editor.

import _ from 'lodash';
import * as http from 'axios';
import Vue from 'vue/dist/vue';


// var editor = ace.edit('input');


// editor.setFontSize(13);
// editor.setTheme('ace/theme/tomorrow');
// editor.setShowPrintMargin(false);
// editor.session.setMode('ace/mode/sqlserver');
// editor.session.setTabSize(2);
// editor.session.setUseSoftTabs(true);

// editor.commands.addCommands([{
//   name: 'run_query',
//   bindKey: {
//     win: 'Ctrl-Enter',
//     mac: 'Command-Enter'
//   },
//   exec: function(editor) {
//     runQuery(editor);
//   }
// }]);



// function runQuery(editor) {
//   var query = _.trim(editor.getSelectedText() || editor.getValue());

//   console.log('runQuery: %s', query);

//   http.post('/api/query', { query })
//     .then(function(res) {
      
//     });
// }



Vue.component('editor', {
  template: '<div :id="editorId"></div>',
  props: ['editorId', 'content'],
  data: function() {
    return {
      editor: Object,
      prevContent: ''
    }
  },
  watch: {
    'content': function(value) {
      if (this.prevContent !== value) {
        this.editor.setValue(value, 1);
      }
    }
  },
  mounted: function() {
    this.editor = window.ace.edit(this.editorId);
    this.editor.setValue(this.content, 1);

    this.editor.getSession().setMode('ace/mode/sqlserver');
    this.editor.setTheme('ace/theme/tomorrow');

    this.editor.on('change', function() {
      this.prevContent = this.editor.getValue();
      this.$emit('change-content', this.editor.getValue());
    })
  }
});


var app = new Vue({
  el: '#app',
  data: {
    results: []
  }
});


