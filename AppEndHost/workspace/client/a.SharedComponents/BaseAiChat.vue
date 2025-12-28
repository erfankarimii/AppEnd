<template>
    <div class="card shadow-sm" style="min-height:440px;">
        <div class="card-header py-2 px-3 d-flex align-items-center gap-2">
            <i class="fa-solid fa-robot text-primary"></i>
            <span class="fw-bold flex-grow-1">AI Chat</span>
           

            <div class="dropdown ai-model-dropdown">
                <button class="btn btn-sm ai-model-dropdown-btn dropdown-toggle dropdown-default" type="button" 
                        id="modelDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="fa-solid fa-microchip me-2"></i>
                    <span class="ai-model-text">{{ selectedModelKey || 'Select a model...' }}</span>
                    <i class="fa-solid fa-chevron-down ms-2 ai-model-chevron"></i>
                </button>
                <ul class="dropdown-menu ai-model-dropdown-menu" aria-labelledby="modelDropdown" style="max-height: 400px; overflow-y: auto;">
                    <template v-if="modelOptions && modelOptions.length > 0">
                        <template v-for="(provider, providerIndex) in modelOptions" :key="providerIndex">
                            <li>
                                <h6 class="dropdown-header ai-model-header">
                                    <i class="fa-solid fa-brain me-2"></i>
                                    {{ provider.Name || 'Unknown Provider' }}
                                </h6>
                            </li>
                            <li v-for="(modelName, modelIndex) in (Array.isArray(provider.Models) ? provider.Models : [])" :key="modelIndex">
                                <a class="dropdown-item ai-model-item" href="#" 
                                   :class="{ 'active': selectedModelKey === modelName }"
                                   @click.stop.prevent="selectModel(modelName, $event)">
                                    <i class="fa-solid fa-robot me-2"></i>
                                    <span class="flex-grow-1">{{ modelName }}</span>
                                    <i v-if="selectedModelKey === modelName" class="fa-solid fa-check text-success ms-2"></i>
                                </a>
                            </li>
                            <li v-if="providerIndex < modelOptions.length - 1"><hr class="dropdown-divider ai-model-divider"></li>
                        </template>
                    </template>
                    <li v-else>
                        <span class="dropdown-item ai-model-item text-muted">
                            <i class="fa-solid fa-spinner fa-spin me-2"></i>
                            <span>Loading models...</span>
                        </span>
                    </li>
                </ul>
            </div>

        </div>
        <div class="card-body p-2 fs-d8 scrollable d-flex flex-column">
            <div class="flex-grow-1 mb-2 pe-1 overflow-auto" ref="messagesContainer">
                <template v-if="messages && messages.length > 0">
                    <div v-for="(m, idx) in messages" :key="'msg-' + idx" class="mb-2">
                        <div class="small text-secondary">{{ m.role === 'user' ? 'You' : 'AI' }} - {{ m.modelLabel || 'AI' }}</div>
                        <div :class="['p-2 rounded', m.role === 'user' ? 'bg-primary text-white' : 'bg-light']">
                            <pre class="m-0" style="white-space: pre-wrap;">{{ m.content }}</pre>
                        </div>
                    </div>
                </template>
                <div v-else class="text-muted small text-center py-4">
                    No messages yet. Start a conversation!
                </div>
            </div>
        </div>
        <div class="card-footer p-2">
            <div class="position-relative">
                <textarea class="form-control ae-focus pe-5" rows="2" v-model="prompt" 
                          @keydown="handleKeydown"
                          placeholder="Type your prompt and press Ctrl+Enter to send..."
                          style="resize:none; border-radius:12px;"></textarea>
                <button class="btn btn-primary btn-sm position-absolute top-50 translate-middle-y rounded rounded-circle" type="button"
                        @click.prevent="send" 
                        :disabled="busy || !trimmedPrompt || !selectedModelKey || selectedModelKey === 'Select a model...'"
                        :class="{ 'disabled': busy || !trimmedPrompt || !selectedModelKey || selectedModelKey === 'Select a model...' }"
                        style="right:12px; z-index:10; cursor: pointer;">
                    <i class="fa-solid fa-paper-plane"></i>
                </button>
            </div>
        </div>
    </div>
</template>

<script>
    let _this = { cid: '', c: null, inputs: {}, prompt: '', messages: [], busy: false, modelOptions: [], selectedModelKey: '' };

    export default {
        computed: {
            trimmedPrompt() {
                // Access prompt directly from Vue reactive data
                let p = (this.$data && this.$data.prompt) || this.prompt || '';
                return p ? String(p).trim() : '';
            }
        },
            methods: {
            selectModel(modelName, event) {
                if (!modelName || modelName === 'Select a model...') {
                    return;
                }
                
                // Update selected model
                this.selectedModelKey = modelName;
                _this.selectedModelKey = modelName;
                
                
                // Close Bootstrap dropdown programmatically
                this.$nextTick(() => {
                    try {
                        const dropdownButton = this.$el?.querySelector('#modelDropdown');
                        const dropdownMenu = this.$el?.querySelector('.dropdown-menu');
                        
                        if (dropdownButton && dropdownMenu) {
                            // Method 1: Try using Bootstrap Dropdown API
                            try {
                                const bootstrapLib = window.bootstrap || (typeof bootstrap !== 'undefined' ? bootstrap : null);
                                if (bootstrapLib && bootstrapLib.Dropdown) {
                                    const dropdownInstance = bootstrapLib.Dropdown.getInstance(dropdownButton);
                                    if (dropdownInstance) {
                                        dropdownInstance.hide();
                                        return;
                                    }
                                }
                            } catch (bsError) {
                                console.log('Bootstrap API not available, using fallback');
                            }
                            
                            // Method 2: Fallback - manually hide dropdown by removing 'show' class and clicking outside
                            dropdownMenu.classList.remove('show');
                            dropdownButton.classList.remove('show');
                            dropdownButton.setAttribute('aria-expanded', 'false');
                            
                            // Trigger a click on the document body to ensure dropdown closes
                            setTimeout(() => {
                                const clickEvent = new MouseEvent('click', {
                                    bubbles: true,
                                    cancelable: true,
                                    view: window
                                });
                                document.body.dispatchEvent(clickEvent);
                            }, 10);
                        }
                    } catch (e) {
                        console.error('Error closing dropdown:', e);
                    }
                });
                
                // Force Vue update to ensure UI reflects the change
                this.$forceUpdate();
            },
            handleKeydown(event) {
                if (event.ctrlKey && (event.key === 'Enter' || event.keyCode === 13)) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.send(event);
                }
            },
            send(event) {
                if (event) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                
                // Get prompt directly from the textarea element to ensure we have the latest value
                let textareaEl = this.$el ? this.$el.querySelector('textarea') : null;
                let rawPrompt = textareaEl ? textareaEl.value : (this.prompt || _this.prompt || '');
                let p = rawPrompt ? String(rawPrompt).trim() : '';
                
                if (this.busy) {
                    return;
                }
                
                if (!p || p === '') {
                    return;
                }
                
                if (!this.selectedModelKey || this.selectedModelKey === 'Select a model...') {
                    showError('Please select a model first');
                    return;
                }

                // Find the provider that contains the selected model
                let selectedProvider = null;
                let selectedModelName = null;
                
                for (let provider of this.modelOptions) {
                    if (Array.isArray(provider.Models) && provider.Models.includes(this.selectedModelKey)) {
                        selectedProvider = provider;
                        selectedModelName = this.selectedModelKey;
                        break;
                    }
                }
                
                if (!selectedProvider || !selectedModelName) {
                    showError('Unknown model selected');
                    return;
                }


                this.busy = true;
                let modelLabel = `${selectedProvider.Name} - ${selectedModelName}`;
                let userMsg = { role: 'user', content: p, model: selectedModelName, provider: selectedProvider.Name, modelLabel: modelLabel };
                
                // Update messages - use Vue reactivity properly
                let currentMessages = Array.isArray(this.messages) ? this.messages : [];
                let newMessages = [...currentMessages, userMsg];
                this.messages = newMessages;
                _this.messages = newMessages;
                this.$forceUpdate();
                
                // Clear prompt from both Vue instance and textarea
                this.prompt = '';
                _this.prompt = '';
                if (textareaEl) textareaEl.value = '';

                rpcAEP('Generate', { prompt: p, model: selectedModelName }, (resp) => {
                    
                    this.busy = false;
                    let content = '';
                    
                    // Parse response if it's a string
                    let parsedResp = resp;
                    if (typeof resp === 'string') {
                        try {
                            parsedResp = JSON.parse(resp);
                        } catch (e) {
                            parsedResp = resp;
                        }
                    }
                    
                    // Extract content from response
                    if (Array.isArray(parsedResp) && parsedResp.length > 0) {
                        let result = R0R(parsedResp);
                        if (typeof result === 'string') {
                            content = result;
                        } else if (result && result.toString) {
                            content = result.toString();
                        } else {
                            content = JSON.stringify(result);
                        }
                    } else if (parsedResp && parsedResp.Result !== undefined) {
                        content = typeof parsedResp.Result === 'string' ? parsedResp.Result : JSON.stringify(parsedResp.Result);
                    } else {
                        content = shared.fixNull(parsedResp, '');
                    }
                    
                    
                    let assistantMsg = { role: 'assistant', content: content, model: selectedModelName, provider: selectedProvider.Name, modelLabel: modelLabel };
                    let currentMessages = Array.isArray(this.messages) ? this.messages : [];
                    let newMessages = [...currentMessages, assistantMsg];
                    this.messages = newMessages;
                    _this.messages = newMessages;
                    this.$forceUpdate();
                    
                    this.$nextTick(() => {
                        this.scrollToBottom();
                    });
                }, (err) => {
                    this.busy = false;
                    let errorMsg = err && err.message ? err.message : (typeof err === 'string' ? err : JSON.stringify(err));
                    showError('AI Error: ' + errorMsg);
                    let errorMsgObj = { role: 'assistant', content: 'Error: ' + errorMsg, model: selectedModelName, provider: selectedProvider.Name, modelLabel: modelLabel };
                    let currentMessages = Array.isArray(this.messages) ? this.messages : [];
                    let newMessages = [...currentMessages, errorMsgObj];
                    this.messages = newMessages;
                    _this.messages = newMessages;
                    this.$forceUpdate();
                });
            },
            scrollToBottom() {
                try {
                    let el = this.$refs.messagesContainer;
                    if (el) el.scrollTop = el.scrollHeight;
                } catch (e) { 
                }
            },
            loadModels() {
                rpcAEP('GetAiProvidersWithModels', {}, (resp) => {
                    // Parse response if it's a string
                    let parsedResp = resp;
                    if (typeof resp === 'string') {
                        try {
                            parsedResp = JSON.parse(resp);
                        } catch (e) {
                        }
                    }
                    
                    let list = R0R(parsedResp);
                    
                    if (!Array.isArray(list)) {
                        showError('Invalid response format. Please check LLM Providers in Settings.');
                        this.modelOptions = [];
                        this.selectedModelKey = '';
                        return;
                    }
                    
                    if (list.length === 0) {
                        showError('No LLM providers configured. Please add providers in AppEnd Settings.');
                        this.modelOptions = [];
                        this.selectedModelKey = '';
                        return;
                    }
                    
                    this.modelOptions = list;
                    _this.modelOptions = list;
                    
                    // Auto-select first model if available
                    if (this.modelOptions.length > 0 && this.modelOptions[0].Models && Array.isArray(this.modelOptions[0].Models) && this.modelOptions[0].Models.length > 0) {
                        this.selectedModelKey = this.modelOptions[0].Models[0];
                        _this.selectedModelKey = this.modelOptions[0].Models[0];
                    } else {
                        this.selectedModelKey = '';
                        _this.selectedModelKey = '';
                    }
                    
                    this.$forceUpdate();
                }, (err) => {
                    showError('Failed to load AI models. Error: ' + (err && err.message ? err.message : JSON.stringify(err)));
                    this.modelOptions = [];
                    this.selectedModelKey = '';
                });
            }
        },
        setup(props) {
            _this.cid = props['cid'];
            _this.inputs = shared['params_' + _this.cid] || {};
            return _this;
        },
        data() { 
            return _this; 
        },
        created() { 
            _this.c = this;
        },
        mounted() {
            initVueComponent(_this);
            // Load models immediately and also after a short delay to ensure API is ready
            this.loadModels();
            setTimeout(() => {
                if (!this.modelOptions || this.modelOptions.length === 0) {
                    this.loadModels();
                }
            }, 500);
        },
        props: { cid: String }
    };
</script>
<style scoped>
/* AI Model Dropdown Styling */
.ai-model-dropdown {
    position: relative;
}

.ai-model-dropdown-btn {
    background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
    border: 1px solid rgba(59, 130, 246, 0.2);
    border-radius: 8px;
    padding: 6px 12px;
    min-width: 200px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    transition: all 0.2s ease;
    color: #495057;
    font-size: 0.875rem;
    font-weight: 500;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.ai-model-dropdown-btn:hover {
    background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
    border-color: rgba(59, 130, 246, 0.4);
    box-shadow: 0 4px 8px rgba(59, 130, 246, 0.1);
    transform: translateY(-1px);
}

.ai-model-dropdown-btn:focus,
.ai-model-dropdown-btn.show {
    background: linear-gradient(135deg, #ffffff 0%, #f0f4ff 100%);
    border-color: rgba(59, 130, 246, 0.5);
    box-shadow: 0 4px 12px rgba(59, 130, 246, 0.15);
    outline: none;
}

.ai-model-text {
    flex: 1;
    text-align: left;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.ai-model-chevron {
    font-size: 0.7rem;
    transition: transform 0.2s ease;
    opacity: 0.6;
}

.ai-model-dropdown-btn.show .ai-model-chevron {
    transform: rotate(180deg);
}

.ai-model-dropdown-menu {
    border-radius: 12px;
    padding: 8px;
    min-width: 280px;
    max-width: 350px;
    max-height: 400px;
    overflow-y: auto;
    box-shadow: 0 10px 30px rgba(15, 23, 42, 0.12);
    border: 1px solid rgba(59, 130, 246, 0.15);
    background: linear-gradient(180deg, #ffffff 0%, #fbfdff 100%);
    margin-top: 6px;
}

.ai-model-header {
    padding: 8px 12px;
    margin: 0;
    font-size: 0.75rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    color: #3b82f6;
    background: rgba(59, 130, 246, 0.05);
    border-radius: 6px;
    margin-bottom: 4px;
}

.ai-model-item {
    padding: 10px 12px;
    margin: 2px 0;
    border-radius: 8px;
    display: flex;
    align-items: center;
    color: #495057;
    font-size: 0.875rem;
    transition: all 0.15s ease;
    cursor: pointer;
    text-decoration: none;
}

.ai-model-item i.fa-robot {
    color: #6c757d;
    width: 18px;
    text-align: center;
    font-size: 0.9rem;
}

.ai-model-item:hover {
    background: linear-gradient(90deg, rgba(59, 130, 246, 0.1) 0%, rgba(59, 130, 246, 0.05) 100%);
    color: #3b82f6;
    transform: translateX(4px);
}

.ai-model-item:hover i.fa-robot {
    color: #3b82f6;
}

.ai-model-item.active {
    background: linear-gradient(90deg, rgba(59, 130, 246, 0.15) 0%, rgba(59, 130, 246, 0.08) 100%);
    color: #3b82f6;
    font-weight: 600;
    border-left: 3px solid #3b82f6;
}

.ai-model-item.active i.fa-robot {
    color: #3b82f6;
}

.ai-model-divider {
    margin: 8px 4px;
    border-top: 1px solid rgba(59, 130, 246, 0.1);
}

/* Scrollbar styling for dropdown */
.ai-model-dropdown-menu::-webkit-scrollbar {
    width: 6px;
}

.ai-model-dropdown-menu::-webkit-scrollbar-track {
    background: #f1f1f1;
    border-radius: 10px;
}

.ai-model-dropdown-menu::-webkit-scrollbar-thumb {
    background: rgba(59, 130, 246, 0.3);
    border-radius: 10px;
}

.ai-model-dropdown-menu::-webkit-scrollbar-thumb:hover {
    background: rgba(59, 130, 246, 0.5);
}
</style>